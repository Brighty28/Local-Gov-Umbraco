using LocalGov.Umbraco.News.Services;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace LocalGov.Umbraco.News.Composers;

public class NewsComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.Services.AddScoped<INewsService, NewsService>();
    }
}
