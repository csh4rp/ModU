namespace ModU.Abstract.Queries;

public interface IQueryProcessor
{
    Task<TResult> HandleAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = new());
}