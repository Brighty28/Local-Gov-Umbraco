using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace LocalGov.Umbraco.StepByStep.Composers;

public class StepByStepComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        // StepByStep module registers no additional DI — views and compositions do the work.
    }
}
