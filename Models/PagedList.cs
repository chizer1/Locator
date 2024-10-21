using System.ComponentModel.DataAnnotations;

namespace Locator.Models;

public class PagedList<T>(
    IEnumerable<T> items,
    int rowCount,
    int pageNumber,
    int pageSize,
    int totalPages
)
{
    [Required]
    public IEnumerable<T> Items { get; set; } = items;

    [Required]
    public int RowCount { get; set; } = rowCount;

    [Required]
    public int PageNumber { get; private set; } = pageNumber;

    [Required]
    public int PageSize { get; private set; } = pageSize;

    [Required]
    public int TotalPages { get; private set; } = totalPages;
}
