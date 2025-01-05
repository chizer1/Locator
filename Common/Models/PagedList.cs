namespace Locator.Common.Models;

/// <summary>
/// Represents a paginated list of items.
/// </summary>
/// <typeparam name="T">The type of items in the list.</typeparam>
/// <remarks>
/// Initializes a new instance of the <see cref="PagedList{T}"/> class.
/// </remarks>
/// <param name="items">The items in the current page.</param>
/// <param name="rowCount">The total number of items.</param>
/// <param name="pageNumber">The current page number.</param>
/// <param name="pageSize">The number of items per page.</param>
/// <param name="totalPages">The total number of pages.</param>
public class PagedList<T>(
    IEnumerable<T> items,
    int rowCount,
    int pageNumber,
    int pageSize,
    int totalPages
)
{
    /// <summary>
    /// Gets or sets the items in the current page.
    /// </summary>
    public IEnumerable<T> Items { get; set; } = items;

    /// <summary>
    /// Gets or sets the total number of items.
    /// </summary>
    public int RowCount { get; set; } = rowCount;

    /// <summary>
    /// Gets the current page number.
    /// </summary>
    public int PageNumber { get; private set; } = pageNumber;

    /// <summary>
    /// Gets the number of items per page.
    /// </summary>
    public int PageSize { get; private set; } = pageSize;

    /// <summary>
    /// Gets the total number of pages.
    /// </summary>
    public int TotalPages { get; private set; } = totalPages;
}
