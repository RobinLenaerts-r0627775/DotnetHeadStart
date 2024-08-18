using DotnetHeadStart.QueryFilters;

namespace DotnetHeadStart.Extensions;

public static class IQueryableExtensions
{
    public static IQueryable<T> Filter<T>(this IQueryable<T> query) where T : class
    {
        // implement filter logic here



        throw new NotImplementedException();
        //TODO: Implement filter logic

        return query;
    }

    public static PaginatedResponse<T> FilterAndPaginate<T>(this IQueryable<T> query, int pageNumber, int pageSize, Expression<Func<T, object>>? SortBy, string SortDirection = "desc") where T : class
    {
        query = query.Filter();
        query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        if (SortBy != null)
        {
            query = SortDirection == "desc" ? query.OrderByDescending(SortBy) : query.OrderBy(SortBy);
        }
        var response = new PaginatedResponse<T>
        {
            Items = [.. query],
            PageNumber = pageNumber,
            PageSize = pageSize,
        };
        return response;
    }
}
