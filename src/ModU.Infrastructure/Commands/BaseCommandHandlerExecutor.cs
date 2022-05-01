namespace ModU.Infrastructure.Commands;

internal abstract class BaseCommandHandlerExecutor
{
    public abstract Task ExecuteHandlerAsync(object handler, object command, CancellationToken cancellationToken);
}

internal abstract class BaseCommandHandlerExecutor<TResult>
{
    public abstract Task<TResult> ExecuteHandlerAsync(object handler, object command, CancellationToken cancellationToken);
}