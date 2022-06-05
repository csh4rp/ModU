using ModU.Abstract.Commands;

namespace ModU.Infrastructure.Commands;

internal interface IUnitOfWork
{
    Task ExecuteAsync<TCommand>(TCommand command, ICommandHandler<TCommand> commandHandler,
        CancellationToken cancellationToken = new()) where TCommand : ICommand;
}