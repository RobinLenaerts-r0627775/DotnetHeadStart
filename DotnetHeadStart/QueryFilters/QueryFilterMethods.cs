namespace DotnetHeadStart.QueryFilters;

public abstract class IQueryFilterMethods
{
    public abstract IQueryable Filter<T>(IQueryable query) where T : class;
}
