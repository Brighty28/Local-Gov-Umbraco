using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace LocalGov.Umbraco.Core.Extensions;

/// <summary>
/// Resolves well-known content tree nodes by document type alias.
/// Each module extends this via its own scoped helper or additional registrations.
/// </summary>
public interface ILocalGovContentHelper
{
    IPublishedContent? GetHome();
    IPublishedContent? GetAlertList();
    IPublishedContent? GetNewsList();
    IPublishedContent? GetEventList();
    IPublishedContent? GetAtoZ();
    IPublishedContent? GetCurrentPage();
    IPublishedContent? FindByDocumentType(string typeAlias);
    IPublishedContent? FindDescendantByDocumentType(string typeAlias);
    IEnumerable<IPublishedContent> GetContentOfType(string typeAlias);
    List<IPublishedContent> GetLiveRecords(List<IPublishedContent> content);
    bool IsValidLink(Link? link);
}
