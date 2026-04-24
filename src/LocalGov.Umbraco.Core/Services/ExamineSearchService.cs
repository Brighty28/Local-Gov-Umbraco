using Examine;
using Examine.Search;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Examine;
using UmbracoConstants = Umbraco.Cms.Core.Constants;

namespace LocalGov.Umbraco.Core.Services;

public class ExamineSearchService(
    IExamineManager examineManager,
    IUmbracoContextAccessor umbracoContextAccessor,
    IPublishedUrlProvider publishedUrlProvider,
    ILogger<ExamineSearchService> logger) : ILocalGovSearchService
{
    public LocalGovSearchResults Search(string query, int page = 1, int pageSize = 10)
    {
        if (string.IsNullOrWhiteSpace(query))
            return new LocalGovSearchResults([], 0, page, pageSize);

        try
        {
            if (!examineManager.TryGetIndex(UmbracoConstants.UmbracoIndexes.ExternalIndexName, out var index))
                return new LocalGovSearchResults([], 0, page, pageSize);

            var searcher = index.Searcher;
            var fields = new[] { "nodeName", "bodyText", "summary", "main" };

            var results = searcher.CreateQuery("content")
                .ManagedQuery(query, fields)
                .Execute(QueryOptions.SkipTake((page - 1) * pageSize, pageSize));

            if (!umbracoContextAccessor.TryGetUmbracoContext(out var ctx))
                return new LocalGovSearchResults([], 0, page, pageSize);

            var items = results
                .Where(r => int.TryParse(r.Id, out _))
                .Select(r =>
                {
                    var id = int.Parse(r.Id);
                    var content = ctx.Content?.GetById(id);
                    if (content == null) return null;
                    return new LocalGovSearchResult(
                        content.Name ?? r.Values.GetValueOrDefault("nodeName", ""),
                        publishedUrlProvider.GetUrl(content),
                        r.Values.GetValueOrDefault("summary", ""),
                        content.ContentType.Alias);
                })
                .Where(r => r != null)
                .Cast<LocalGovSearchResult>();

            return new LocalGovSearchResults(items, (int)results.TotalItemCount, page, pageSize);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "LocalGov search error for query: {Query}", query);
            return new LocalGovSearchResults([], 0, page, pageSize);
        }
    }
}
