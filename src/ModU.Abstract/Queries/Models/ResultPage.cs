namespace ModU.Abstract.Queries.Models;

public sealed class ResultPage<T>
{
    public ResultPage(IReadOnlyCollection<T> items, int pageIndex, int pageSize, int totalItems)
    {
        Items = items;
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalItems = totalItems;
        TotalPages = (int) Math.Ceiling(totalItems / (double)pageSize);
    }

    public IReadOnlyCollection<T> Items { get; }
    public int PageIndex { get; }
    public int PageSize { get; }
    public int TotalItems { get; }
    public int TotalPages { get; }
}