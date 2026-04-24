using Umbraco.Cms.Core.Models.PublishedContent;

namespace LocalGov.Umbraco.AlertBanner.Services;

public interface IAlertService
{
    IEnumerable<AlertModel> GetAlertsForPage(IPublishedContent currentPage);
}

public record AlertModel(
    string Title,
    string? Body,
    AlertSeverity Severity,
    bool IsDismissible,
    string DismissalKey);

public enum AlertSeverity
{
    Announcement,
    Minor,
    Major,
    NotableDeath
}
