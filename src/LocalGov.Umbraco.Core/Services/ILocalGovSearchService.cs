namespace LocalGov.Umbraco.Core.Services;

public interface ILocalGovSearchService
{
    LocalGovSearchResults Search(string query, int page = 1, int pageSize = 10);
}

public record LocalGovSearchResults(IEnumerable<LocalGovSearchResult> Items, int TotalItems, int CurrentPage, int PageSize)
{
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
}

public record LocalGovSearchResult(string Name, string Url, string? Summary, string ContentTypeAlias);
