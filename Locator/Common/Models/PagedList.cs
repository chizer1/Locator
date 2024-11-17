namespace Locator.Common.Models;

public class PagedList<T>(
    IEnumerable<T> items,
    int rowCount,
    int pageNumber,
    int pageSize,
    int totalPages
)
{
    public IEnumerable<T> Items { get; set; } = items;

    public int RowCount { get; set; } = rowCount;

    public int PageNumber { get; private set; } = pageNumber;

    public int PageSize { get; private set; } = pageSize;

    public int TotalPages { get; private set; } = totalPages;
}
