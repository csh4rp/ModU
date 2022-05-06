using ModU.Abstract.Commands;

namespace ModU.Infrastructure.Commands;

internal interface IUnitOffWork
{
    Task ExecuteAsync<TCommand>(TCommand command, ICommandHandler<TCommand> commandHandler,
        CancellationToken cancellationToken = new()) where TCommand : ICommand;
    
    Task<TResult> ExecuteAsync<TCommand, TResult>(TCommand command, ICommandHandler<TCommand, TResult> commandHandler,
        CancellationToken cancellationToken = new()) where TCommand : ICommand<TResult>;
}