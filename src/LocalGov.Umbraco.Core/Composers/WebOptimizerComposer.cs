using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Web.Common.ApplicationBuilder;
using WebOptimizer;

namespace LocalGov.Umbraco.Core.Composers;

/// <summary>
/// Registers the LocalGov asset pipeline (CSS + JS bundling, minification, fingerprinting)
/// using LigerShark.WebOptimizer. Replaces Umbraco's default Smidge usage for the LocalGov
/// theme assets.
///
/// Theme selection is config-driven — set <c>LocalGov:Theme</c> in appsettings.json:
///
/// <code>
/// "LocalGov": { "Theme": "default" }    // ships GOV.UK Frontend modernised (default)
/// "LocalGov": { "Theme": "verdant" }    // forest-green / nature aesthetic
/// </code>
///
/// The chosen theme name maps directly to a CSS file in
/// <c>LocalGov.Umbraco.Theme/wwwroot/localgov/theme/{theme}.css</c>. "default" is
/// special-cased to <c>localgov.css</c> for backward compatibility with v1.0/1.1.
///
/// Bundles produced:
///   /css/localgov.css — GOV.UK Frontend + selected theme stylesheet
///   /js/localgov.js   — GOV.UK Frontend ES module (passed through for cache-busting)
/// </summary>
public class WebOptimizerComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        var theme = builder.Config.GetValue<string>("LocalGov:Theme") ?? "default";
        var themeFile = string.Equals(theme, "default", StringComparison.OrdinalIgnoreCase)
            ? "localgov.css"
            : $"{theme}.css";

        builder.Services.AddWebOptimizer(pipeline =>
        {
            pipeline.AddCssBundle(
                "/css/localgov.css",
                "/_content/LocalGov.Umbraco.Theme/localgov/theme/govuk-frontend.min.css",
                $"/_content/LocalGov.Umbraco.Theme/localgov/theme/{themeFile}");

            pipeline.AddJavaScriptBundle(
                "/js/localgov.js",
                "/_content/LocalGov.Umbraco.Theme/localgov/theme/govuk-frontend.min.js");
        });

        // Insert the WebOptimizer middleware ahead of Umbraco's static file pipeline
        // so /css/localgov.css and /js/localgov.js are served by the bundler before
        // the static-files middleware would 404 on them.
        builder.Services.Configure<UmbracoPipelineOptions>(options =>
        {
            options.AddFilter(new UmbracoPipelineFilter("LocalGovWebOptimizer")
            {
                PrePipeline = app => app.UseWebOptimizer()
            });
        });
    }
}
