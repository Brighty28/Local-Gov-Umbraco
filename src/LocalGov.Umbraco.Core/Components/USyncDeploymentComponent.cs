using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Umbraco.Cms.Core.Composing;

namespace LocalGov.Umbraco.Core.Components;

/// <summary>
/// Materialises embedded uSync config files from installed LocalGov packages
/// to the site's uSync folder on startup, enabling uSync to discover them.
/// </summary>
public class USyncDeploymentComponent(IHostEnvironment hostEnvironment, ILogger<USyncDeploymentComponent> logger) : IComponent
{
    private const string USyncPathPrefix = "uSync/v13/";

    public void Initialize()
    {
        var siteRoot = hostEnvironment.ContentRootPath;
        var assemblies = GetLocalGovAssemblies();

        foreach (var assembly in assemblies)
        {
            DeployUSyncFilesFromAssembly(assembly, siteRoot);
        }
    }

    public void Terminate() { }

    private static IEnumerable<Assembly> GetLocalGovAssemblies() =>
        AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.FullName?.StartsWith("LocalGov.Umbraco.", StringComparison.OrdinalIgnoreCase) == true);

    private void DeployUSyncFilesFromAssembly(Assembly assembly, string siteRoot)
    {
        var resourceNames = assembly.GetManifestResourceNames()
            .Where(n => n.Contains(".uSync.v13."));

        foreach (var resourceName in resourceNames)
        {
            try
            {
                var filePath = ResourceNameToFilePath(resourceName, assembly, siteRoot);
                if (filePath == null || File.Exists(filePath)) continue;

                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
                using var stream = assembly.GetManifestResourceStream(resourceName)!;
                using var fileStream = File.Create(filePath);
                stream.CopyTo(fileStream);

                logger.LogDebug("LocalGov uSync: deployed {File}", filePath);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "LocalGov uSync: failed to deploy resource {Resource}", resourceName);
            }
        }
    }

    private static string? ResourceNameToFilePath(string resourceName, Assembly assembly, string siteRoot)
    {
        // e.g. LocalGov.Umbraco.Core.uSync.v13.ContentTypes.lgHome.config
        // → uSync/v13/ContentTypes/lgHome.config
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
        // Path.DirectorySeparatorChar isn't a compile-time constant so we use a runtime check
        var sep = Path.DirectorySeparatorChar.ToString();
        if (path.EndsWith(sep + "config", StringComparison.OrdinalIgnoreCase))
            return path[..^(sep.Length + "config".Length)] + ".config";
        return path;
    }
}
