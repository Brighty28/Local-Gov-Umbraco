using Microsoft.AspNetCore.Mvc;

namespace LocalGov.Umbraco.Core.ViewComponents;

public class PaginationViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(int currentPage, int totalPages, string baseUrl)
    {
        if (totalPages <= 1) return Content(string.Empty);

        var model = new PaginationModel(currentPage, totalPages, baseUrl);
        return View(model);
    }
}

public record PaginationModel(int CurrentPage, int TotalPages, string BaseUrl)
{
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;
    public int PreviousPage => CurrentPage - 1;
    public int NextPage => CurrentPage + 1;

    public IEnumerable<int?> PageNumbers()
    {
        // Returns page numbers with nulls representing ellipsis
        for (int i = 1; i <= TotalPages; i++)
        {
            if (i == 1 || i == TotalPages || Math.Abs(i - CurrentPage) <= 2)
                yield return i;
            else if (Math.Abs(i - CurrentPage) == 3)
                yield return null;
        }
    }
}
