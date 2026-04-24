using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace LocalGov.Umbraco.Guides.ViewComponents;

public class GuideNavViewModel
{
    public IPublishedContent? Guide { get; init; }
    public IList<IPublishedContent> Pages { get; init; } = new List<IPublishedContent>();
    public IPublishedContent? CurrentPage { get; init; }
}

public class GuideNavViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(IPublishedContent currentPage)
    {
        var guide = currentPage.ContentType.Alias.Equals("lgGuide", StringComparison.OrdinalIgnoreCase)
            ? currentPage
            : currentPage.Parent;

        var pages = guide?.Children?.Where(c => c.IsVisible()).ToList()
                    ?? new List<IPublishedContent>();

        return View(new GuideNavViewModel { Guide = guide, Pages = pages, CurrentPage = currentPage });
    }
}
