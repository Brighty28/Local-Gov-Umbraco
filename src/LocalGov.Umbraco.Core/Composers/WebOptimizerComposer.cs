using Microsoft.AspNetCore.Builder;
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
/// Bundles produced:
///   /css/localgov.css — GOV.UK Frontend + LocalGov compiled theme
///   /js/localgov.js   — GOV.UK Frontend ES module (passed through for cache-busting)
///
/// Source files come from the LocalGov.Umbraco.Theme RCL via the static web assets manifest.
/// </summary>
public class WebOptimizerComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.Services.AddWebOptimizer(pipeline =>
        {
            pipeline.AddCssBundle(
                "/css/localgov.css",
                "/_content/LocalGov.Umbraco.Theme/localgov/theme/govuk-frontend.min.css",
                "/_content/LocalGov.Umbraco.Theme/localgov/theme/localgov.css");

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
