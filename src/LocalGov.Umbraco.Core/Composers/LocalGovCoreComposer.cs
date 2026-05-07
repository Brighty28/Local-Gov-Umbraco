using LocalGov.Umbraco.Core.Components;
using LocalGov.Umbraco.Core.Extensions;
using LocalGov.Umbraco.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Notifications;

namespace LocalGov.Umbraco.Core.Composers;

public class LocalGovCoreComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.Services.AddTransient<ILocalGovContentHelper, LocalGovContentHelper>();
        builder.Services.AddScoped<ILocalGovSearchService, ExamineSearchService>();

        // Deploy embedded uSync configs as early as possible so that uSync's
        // own import (which fires on UmbracoApplicationStartedNotification) sees
        // all config files — including composition content types — on disk.
        builder.AddNotificationHandler<
            UmbracoApplicationStartingNotification,
            USyncDeploymentComponent>();
    }
}
