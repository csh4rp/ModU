namespace ModU.Abstract.Commands;

public interface ICommandProcessor
{
    Task ProcessAsync<TCommand>(TCommand command, CancellationToken cancellationToken = new()) where TCommand : ICommand;
}