using ModU.Abstract.Queries.Models;

namespace ModU.Abstract.Queries;

public abstract class PagedQuery<TResult> : IQuery<ResultPage<TResult>>
{
    protected PagedQuery(int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
    }

    public int PageIndex { get; }
    public int PageSize { get; }
}