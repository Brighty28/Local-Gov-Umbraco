using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace LocalGov.Umbraco.Core.Components;

/// <summary>
/// Materialises embedded uSync config files from all installed LocalGov packages
/// to the site's uSync folder. Runs via UmbracoApplicationStartingNotification —
/// the earliest startup hook — so the files are on disk before uSync's import
/// notification handler fires, ensuring compositions are resolved correctly.
/// </summary>
public class USyncDeploymentComponent(
    IHostEnvironment hostEnvironment,
    ILogger<USyncDeploymentComponent> logger)
    : INotificationHandler<UmbracoApplicationStartingNotification>
{
    public void Handle(UmbracoApplicationStartingNotification notification)
    {
        var siteRoot = hostEnvironment.ContentRootPath;
        var assemblies = GetLocalGovAssemblies().ToList();

        logger.LogInformation(
            "LocalGov uSync: deploying configs from {Count} assemblies to {Root}",
            assemblies.Count, siteRoot);

        var totalWritten = 0;
        foreach (var assembly in assemblies)
            totalWritten += DeployUSyncFilesFromAssembly(assembly, siteRoot);

        logger.LogInformation(
            "LocalGov uSync: deployment complete — wrote {Count} config files",
            totalWritten);
    }

    private static IEnumerable<Assembly> GetLocalGovAssemblies() =>
        AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.FullName?.StartsWith("LocalGov.Umbraco.",
                StringComparison.OrdinalIgnoreCase) == true);

    private int DeployUSyncFilesFromAssembly(Assembly assembly, string siteRoot)
    {
        var resourceNames = assembly.GetManifestResourceNames()
            .Where(n => n.Contains(".uSync.v13."))
            .ToList();

        if (resourceNames.Count == 0)
            return 0;

        var written = 0;
        foreach (var resourceName in resourceNames)
        {
            try
            {
                var filePath = ResourceNameToFilePath(resourceName, assembly, siteRoot);
                if (filePath == null) continue;

                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

                // Always overwrite — keeps deployed files in sync with the
                // currently-installed package version and corrects case-
                // mismatched leftovers from earlier installs on Windows.
                using var stream = assembly.GetManifestResourceStream(resourceName)!;
                using var fileStream = File.Create(filePath);
                stream.CopyTo(fileStream);
                written++;

                logger.LogInformation("LocalGov uSync: wrote {File}", filePath);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex,
                    "LocalGov uSync: failed to deploy resource {Resource}", resourceName);
            }
        }
        return written;
    }

    private static string? ResourceNameToFilePath(
        string resourceName, Assembly assembly, string siteRoot)
    {
        // e.g. LocalGov.Umbraco.Core.uSync.v13.ContentTypes.lgHome.config
        //   →  uSync/v13/ContentTypes/lgHome.config
        var ns = assembly.GetName().Name + ".";
        if (!resourceName.StartsWith(ns)) return null;

        var relative = resourceName[ns.Length..]
            .Replace('.', Path.DirectorySeparatorChar);

        // Restore the .config extension (last segment lost its dot above)
        relative = RestoreConfigExtension(relative);

        return Path.Combine(siteRoot, relative);
    }

    private static string RestoreConfigExtension(string path)
    {
        var sep = Path.DirectorySeparatorChar.ToString();
        if (path.EndsWith(sep + "config", StringComparison.OrdinalIgnoreCase))
            return path[..^(sep.Length + "config".Length)] + ".config";
        return path;
    }
}
