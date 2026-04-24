using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common;
using Umbraco.Extensions;

namespace LocalGov.Umbraco.Core.Extensions;

public class LocalGovContentHelper(UmbracoHelper umbracoHelper) : ILocalGovContentHelper
{
    public IPublishedContent? GetHome() =>
        umbracoHelper.ContentAtRoot().FirstOrDefault(p => p.ContentType.Alias.Equals("lgHome", StringComparison.OrdinalIgnoreCase));

    public IPublishedContent? GetAlertList() =>
        umbracoHelper.ContentAtRoot().FirstOrDefault(p => p.ContentType.Alias.Equals("lgAlertList", StringComparison.OrdinalIgnoreCase));

    public IPublishedContent? GetNewsList() =>
        umbracoHelper.ContentAtRoot().FirstOrDefault(p => p.ContentType.Alias.Equals("lgNewsList", StringComparison.OrdinalIgnoreCase))
        ?? umbracoHelper.ContentAtRoot().FirstOrDefault()?.DescendantsOrSelf()
            .FirstOrDefault(p => p.ContentType.Alias.Equals("lgNewsList", StringComparison.OrdinalIgnoreCase));

    public IPublishedContent? GetEventList() =>
        umbracoHelper.ContentAtRoot().FirstOrDefault(p => p.ContentType.Alias.Equals("lgEventList", StringComparison.OrdinalIgnoreCase))
        ?? umbracoHelper.ContentAtRoot().FirstOrDefault()?.DescendantsOrSelf()
            .FirstOrDefault(p => p.ContentType.Alias.Equals("lgEventList", StringComparison.OrdinalIgnoreCase));

    public IPublishedContent? GetAtoZ() =>
        umbracoHelper.ContentAtRoot().FirstOrDefault()?.DescendantsOrSelf()
            .FirstOrDefault(p => p.ContentType.Alias.Equals("lgAtoZ", StringComparison.OrdinalIgnoreCase));

    public IPublishedContent? GetCurrentPage() => umbracoHelper.AssignedContentItem;

    public IPublishedContent? FindByDocumentType(string typeAlias) =>
        umbracoHelper.ContentAtRoot()
            .FirstOrDefault(p => p.ContentType.Alias.Equals(typeAlias, StringComparison.OrdinalIgnoreCase));

    public IPublishedContent? FindDescendantByDocumentType(string typeAlias) =>
        umbracoHelper.ContentAtRoot().FirstOrDefault()?.DescendantsOrSelf()
            .FirstOrDefault(p => p.ContentType.Alias.Equals(typeAlias, StringComparison.OrdinalIgnoreCase));

    public IEnumerable<IPublishedContent> GetContentOfType(string typeAlias) =>
        umbracoHelper.ContentAtRoot()
            .SelectMany(r => r.DescendantsOrSelf())
            .Where(p => p.ContentType.Alias.Equals(typeAlias, StringComparison.OrdinalIgnoreCase));

    public List<IPublishedContent> GetLiveRecords(List<IPublishedContent> content) =>
        content.Where(item => item.IsVisible()).ToList();

    public bool IsValidLink(Link? link) =>
        link != null && !string.IsNullOrWhiteSpace(link.Url) && !string.IsNullOrWhiteSpace(link.Name);
}
