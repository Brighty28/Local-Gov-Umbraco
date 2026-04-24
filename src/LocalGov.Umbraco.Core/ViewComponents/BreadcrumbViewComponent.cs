using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common.PublishedModels;
using Umbraco.Extensions;

namespace LocalGov.Umbraco.Core.ViewComponents;

public class BreadcrumbViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(IPublishedContent currentPage)
    {
        var crumbs = currentPage
            .AncestorsOrSelf()
            .Reverse()
            .Select(p => new BreadcrumbItem(p.Name ?? "", p.Url(), p.Id == currentPage.Id))
            .ToList();

        return View(crumbs);
    }
}

public record BreadcrumbItem(string Name, string Url, bool IsCurrent);
