namespace DotnetHeadStart.QueryFilters;

public sealed class PaginatedResponse<T>
{
    public string? SortOn { get; set; }
    public string SortDirection { get; set; } = "desc";
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public List<T> Items { get; set; } = [];
}
