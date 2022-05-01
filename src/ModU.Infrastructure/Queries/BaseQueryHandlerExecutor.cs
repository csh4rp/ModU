namespace ModU.Infrastructure.Queries;

internal abstract class BaseQueryHandlerExecutor<TResult>
{
    public abstract Task<TResult> ExecuteHandlerAsync(object handler, object query, CancellationToken cancellationToken);
}