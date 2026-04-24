using Umbraco.Cms.Core.Models.PublishedContent;

namespace LocalGov.Umbraco.News.Services;

public interface INewsService
{
    NewsListResult GetNews(int page = 1, int pageSize = 10, string? category = null, int? year = null);
    IEnumerable<IPublishedContent> GetLatestNews(int count = 3);
}

public record NewsListResult(IEnumerable<IPublishedContent> Items, int TotalItems, int CurrentPage, int PageSize)
{
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
}
