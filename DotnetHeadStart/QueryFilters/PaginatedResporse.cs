namespace DotnetHeadStart.QueryFilters;

public sealed class PaginatedResponse<T>
{
    public string SortOn { get; set; }
    public string SortDirection { get; set; } = "desc";
    public List<T> Items { get; set; } = [];
}
