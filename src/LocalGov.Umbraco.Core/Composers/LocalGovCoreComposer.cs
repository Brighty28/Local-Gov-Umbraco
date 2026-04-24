using LocalGov.Umbraco.Core.Components;
using LocalGov.Umbraco.Core.Extensions;
using LocalGov.Umbraco.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace LocalGov.Umbraco.Core.Composers;

public class LocalGovCoreComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.Services.AddTransient<ILocalGovContentHelper, LocalGovContentHelper>();
        builder.Services.AddScoped<ILocalGovSearchService, ExamineSearchService>();

        builder.Components().Append<USyncDeploymentComponent>();
    }
}
