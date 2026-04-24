using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace LocalGov.Umbraco.Guides.Composers;

public class GuidesComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        // Guides module registers no additional DI — views and compositions do the work.
    }
}
