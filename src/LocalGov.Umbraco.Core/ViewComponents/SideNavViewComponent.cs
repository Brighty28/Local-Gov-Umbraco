using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common.PublishedModels;
using Umbraco.Extensions;

namespace LocalGov.Umbraco.Core.ViewComponents;

public class SideNavViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(IPublishedContent currentPage)
    {
        var root = GetNavRoot(currentPage);
        if (root == null) return Content(string.Empty);

        var model = new SideNavModel(
            Root: new SideNavItem(root.Name ?? "", root.Url(), root.Id == currentPage.Id),
            Items: root.Children
                .Where(c => c.IsVisible())
                .Select(c => BuildNavItem(c, currentPage))
                .ToList());

        return View(model);
    }

    private static IPublishedContent? GetNavRoot(IPublishedContent page)
    {
        // Walk up to find the first ancestor that is a service landing (2 levels from home)
        var ancestors = page.AncestorsOrSelf().ToList();
        return ancestors.Count >= 2 ? ancestors[^2] : ancestors.LastOrDefault();
    }

    private static SideNavItem BuildNavItem(IPublishedContent item, IPublishedContent currentPage)
    {
        var isActive = item.Id == currentPage.Id
            || currentPage.Path.Split(',').Contains(item.Id.ToString());
        return new(item.Name ?? "", item.Url(), isActive,
            item.Children.Where(c => c.IsVisible())
                .Select(c => BuildNavItem(c, currentPage)).ToList());
    }
}

public record SideNavModel(SideNavItem Root, List<SideNavItem> Items);
public record SideNavItem(string Name, string Url, bool IsActive, List<SideNavItem>? Children = null);
