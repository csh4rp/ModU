namespace ModU.Abstract.Commands;

public interface ICommandProcessor
{
    Task ProcessAsync(ICommand command, CancellationToken cancellationToken = new());

    Task<TResult> ProcessAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = new());
}