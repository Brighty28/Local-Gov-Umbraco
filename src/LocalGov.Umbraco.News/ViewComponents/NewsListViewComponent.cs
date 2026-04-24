using LocalGov.Umbraco.News.Services;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace LocalGov.Umbraco.News.ViewComponents;

public class NewsListViewComponent(INewsService newsService) : ViewComponent
{
    public IViewComponentResult Invoke(int page = 1, int pageSize = 10, string? category = null, int? year = null)
    {
        var result = newsService.GetNews(page, pageSize, category, year);
        return View(result);
    }
}

public class HomepageNewsListViewComponent(INewsService newsService) : ViewComponent
{
    public IViewComponentResult Invoke(int count = 3)
    {
        var items = newsService.GetLatestNews(count);
        return View(items);
    }
}
