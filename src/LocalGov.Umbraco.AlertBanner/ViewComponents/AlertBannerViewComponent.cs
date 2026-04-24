using LocalGov.Umbraco.AlertBanner.Services;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace LocalGov.Umbraco.AlertBanner.ViewComponents;

public class AlertBannerViewComponent(IAlertService alertService) : ViewComponent
{
    public IViewComponentResult Invoke(IPublishedContent currentPage)
    {
        var alerts = alertService.GetAlertsForPage(currentPage).ToList();
        if (!alerts.Any()) return Content(string.Empty);
        return View(alerts);
    }
}
