using ModU.Abstract.Commands;

namespace ModU.Infrastructure.Commands.Decorators;

internal sealed class TransactionalDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
{
    private readonly ICommandHandler<TCommand> _handler;

    public TransactionalDecorator(ICommandHandler<TCommand> handler) => _handler = handler;

    public async Task HandleAsync(TCommand command, CancellationToken cancellationToken = new())
    {
        await _handler.HandleAsync(command, cancellationToken);
    }
}