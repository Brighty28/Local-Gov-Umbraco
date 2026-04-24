using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace LocalGov.Umbraco.Services.ViewComponents;

public class ServiceNavCardsViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(IPublishedContent page)
    {
        var cards = page.Children?.Where(c => c.IsVisible()) ?? Enumerable.Empty<IPublishedContent>();
        return View(cards);
    }
}
