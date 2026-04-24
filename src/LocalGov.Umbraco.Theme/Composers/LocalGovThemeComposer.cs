using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.Reflection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace LocalGov.Umbraco.Theme.Composers;

public class LocalGovThemeComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        // Static files from this RCL are served at /_content/LocalGov.Umbraco.Theme/
        // This is handled automatically by UseStaticWebAssets() in the consuming app.
        // No additional registration needed for RCL static assets in .NET 6+.
    }
}
