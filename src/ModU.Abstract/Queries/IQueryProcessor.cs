namespace ModU.Abstract.Queries;

public interface IQueryProcessor
{
    Task<TResult> ProcessAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = new());
}