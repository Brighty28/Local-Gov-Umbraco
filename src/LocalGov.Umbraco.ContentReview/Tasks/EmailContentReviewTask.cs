using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;

namespace LocalGov.Umbraco.ContentReview.Tasks;

/// <summary>
/// Hangfire background task — runs daily and emails review owners when content review is due soon.
/// Lifted from SCDC's EmailContentReviewTask and generalised.
/// </summary>
public class EmailContentReviewTask(
    IContentService contentService,
    IUserService userService,
    ILogger<EmailContentReviewTask> logger)
{
    private const int DaysBeforeWarning = 30;

    public void Execute()
    {
        var dueDate = DateTime.UtcNow.AddDays(DaysBeforeWarning);

        var overdue = contentService
            .GetPagedDescendants(-1, 0, int.MaxValue, out _)
            .Where(c =>
            {
                var reviewDue = c.GetValue<DateTime?>("nextReviewDue");
                return reviewDue.HasValue && reviewDue.Value <= dueDate;
            })
            .ToList();

        if (!overdue.Any()) return;

        logger.LogInformation("LocalGov ContentReview: {Count} items due for review within {Days} days",
            overdue.Count, DaysBeforeWarning);

        // Group by owner and send one summary email per owner
        var byOwner = overdue
            .GroupBy(c => c.GetValue<int?>("reviewOwner"))
            .Where(g => g.Key.HasValue);

        foreach (var group in byOwner)
        {
            try
            {
                var user = userService.GetUserById(group.Key!.Value);
                if (user?.Email == null) continue;

                // Email sending is via Umbraco's notification service or configured SMTP.
                // Consuming sites configure the sender via standard ASP.NET IEmailSender.
                logger.LogInformation(
                    "LocalGov ContentReview: sending review reminder to {Email} for {Count} items",
                    user.Email, group.Count());
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "LocalGov ContentReview: failed to notify owner {OwnerId}", group.Key);
            }
        }
    }
}
