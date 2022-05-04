using ModU.Abstract.Commands;

namespace ModU.Infrastructure.Commands.Decorators;

internal sealed class TransactionalDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
{
    private readonly ICommandHandler<TCommand> _handler;
    
    public async Task HandleAsync(TCommand command, CancellationToken cancellationToken = new())
    {
        return;
    }
}