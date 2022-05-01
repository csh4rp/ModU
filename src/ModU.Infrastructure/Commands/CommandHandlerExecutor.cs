using ModU.Abstract.Commands;

namespace ModU.Infrastructure.Commands;

internal sealed class CommandHandlerExecutor<TCommand> : BaseCommandHandlerExecutor where TCommand : ICommand
{
    public override Task ExecuteHandlerAsync(object handler, object command, CancellationToken cancellationToken)
    {
        var handlerToExecute = (ICommandHandler<TCommand>)handler;
        var commandToExecute = (TCommand)command;
        return handlerToExecute.HandleAsync(commandToExecute, cancellationToken);
    }
}

internal sealed class CommandHandlerExecutor<TCommand, TResult> : BaseCommandHandlerExecutor<TResult> where TCommand : ICommand<TResult>
{
    public override Task<TResult> ExecuteHandlerAsync(object handler, object command, CancellationToken cancellationToken)
    {
        var handlerToExecute = (ICommandHandler<TCommand, TResult>)handler;
        var commandToExecute = (TCommand)command;
        return handlerToExecute.HandleAsync(commandToExecute, cancellationToken);
    }
}