using LocalGov.Umbraco.ContentReview.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace LocalGov.Umbraco.ContentReview.Composers;

public class ContentReviewComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.Services.AddTransient<EmailContentReviewTask>();
    }
}
