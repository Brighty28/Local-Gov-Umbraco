using LocalGov.Umbraco.AlertBanner.Services;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace LocalGov.Umbraco.AlertBanner.Composers;

public class AlertBannerComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.Services.AddScoped<IAlertService, AlertService>();
    }
}
