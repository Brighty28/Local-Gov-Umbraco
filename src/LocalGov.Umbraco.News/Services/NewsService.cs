using LocalGov.Umbraco.Core.Extensions;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace LocalGov.Umbraco.News.Services;

public class NewsService(ILocalGovContentHelper contentHelper) : INewsService
{
    public NewsListResult GetNews(int page = 1, int pageSize = 10, string? category = null, int? year = null)
    {
        var newsList = contentHelper.GetNewsList();
        if (newsList == null) return new NewsListResult([], 0, page, pageSize);

        var items = newsList.Children
            .Where(n => n.ContentType.Alias.Equals("lgNewsItem", StringComparison.OrdinalIgnoreCase)
                && n.IsVisible())
            .Where(n => category == null || n.Value<IEnumerable<IPublishedContent>>("newsCategory")
                ?.Any(c => c.UrlSegment == category) == true)
            .Where(n => year == null || (n.Value<DateTime?>("publishDate") ?? n.CreateDate).Year == year)
            .OrderByDescending(n => n.Value<DateTime?>("publishDate") ?? n.CreateDate)
            .ToList();

        var total = items.Count;
        var paged = items.Skip((page - 1) * pageSize).Take(pageSize);

        return new NewsListResult(paged, total, page, pageSize);
    }

    public IEnumerable<IPublishedContent> GetLatestNews(int count = 3) =>
        contentHelper.GetNewsList()?.Children
            .Where(n => n.ContentType.Alias.Equals("lgNewsItem", StringComparison.OrdinalIgnoreCase) && n.IsVisible())
            .OrderByDescending(n => n.Value<DateTime?>("publishDate") ?? n.CreateDate)
            .Take(count)
        ?? [];
}
