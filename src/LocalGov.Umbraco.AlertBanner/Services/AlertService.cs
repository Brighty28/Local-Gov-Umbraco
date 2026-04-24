using LocalGov.Umbraco.Core.Extensions;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace LocalGov.Umbraco.AlertBanner.Services;

public class AlertService(ILocalGovContentHelper contentHelper) : IAlertService
{
    private static readonly string[] SeverityOrder = ["notable-death", "major", "minor", "announcement"];

    public IEnumerable<AlertModel> GetAlertsForPage(IPublishedContent currentPage)
    {
        var alertList = contentHelper.GetAlertList();
        if (alertList == null) return [];

        var now = DateTime.UtcNow;

        // Collect all live alert items from the alert list container
        var allAlerts = alertList.Children
            .Where(a => a.IsVisible()
                && a.ContentType.Alias.Equals("lgAlertItem", StringComparison.OrdinalIgnoreCase)
                && IsScheduledActive(a, now))
            .ToList();

        // Filter to those applicable to this page:
        // - multi-page alerts: on this page OR any ancestor
        // - single-page alerts: only this page
        var pageIds = currentPage.AncestorsOrSelf().Select(p => p.Key).ToHashSet();

        var applicable = allAlerts.Where(alert =>
        {
            var multiPage = alert.Value<IEnumerable<IPublishedContent>>("multiPageAlerts") ?? [];
            var singlePage = alert.Value<IEnumerable<IPublishedContent>>("singlePageAlerts") ?? [];

            return multiPage.Any(p => pageIds.Contains(p.Key))
                || singlePage.Any(p => p.Key == currentPage.Key);
        });

        return applicable
            .OrderBy(a => Array.IndexOf(SeverityOrder, a.Value<string>("severity")?.ToLowerInvariant() ?? "announcement"))
            .Select(a => new AlertModel(
                Title: a.Value<string>("alertTitle") ?? a.Name ?? "",
                Body: a.Value<string>("alertBody"),
                Severity: ParseSeverity(a.Value<string>("severity")),
                IsDismissible: a.Value<string>("severity") is not ("major" or "notable-death"),
                DismissalKey: $"localgov-alert-{a.Key}"));
    }

    private static bool IsScheduledActive(IPublishedContent alert, DateTime now)
    {
        var from = alert.Value<DateTime?>("publishFrom");
        var until = alert.Value<DateTime?>("publishUntil");

        if (from.HasValue && now < from.Value) return false;
        if (until.HasValue && now > until.Value) return false;
        return true;
    }

    private static AlertSeverity ParseSeverity(string? severity) => severity?.ToLowerInvariant() switch
    {
        "major" => AlertSeverity.Major,
        "minor" => AlertSeverity.Minor,
        "notable-death" => AlertSeverity.NotableDeath,
        _ => AlertSeverity.Announcement
    };
}
