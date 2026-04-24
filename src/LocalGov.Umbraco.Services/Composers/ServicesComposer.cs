using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace LocalGov.Umbraco.Services.Composers;

public class ServicesComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        // Services module registers no additional DI — views and compositions do the work.
        // Extension modules (e.g. ModernGov integration) hook in here via additional composers.
    }
}
