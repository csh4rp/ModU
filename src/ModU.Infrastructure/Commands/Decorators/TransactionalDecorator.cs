using ModU.Abstract.Commands;
using ModU.Infrastructure.Modules;

namespace ModU.Infrastructure.Commands.Decorators;

internal sealed class TransactionalDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
{
    private readonly ICommandHandler<TCommand> _handler;
    private readonly ModuleServiceProvider _moduleServiceProvider;

    public TransactionalDecorator(ICommandHandler<TCommand> handler, ModuleServiceProvider moduleServiceProvider)
    {
        _handler = handler;
        _moduleServiceProvider = moduleServiceProvider;
    }

    public Task HandleAsync(TCommand command, CancellationToken cancellationToken = new())
    {
        var unitOfWork = _moduleServiceProvider.GetUnitOfWorkForType(typeof(TCommand));
        return unitOfWork.ExecuteAsync(command, _handler, cancellationToken);
    }
}