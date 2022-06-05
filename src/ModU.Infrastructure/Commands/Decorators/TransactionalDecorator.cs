using ModU.Abstract.Commands;
using ModU.Infrastructure.Modules;

namespace ModU.Infrastructure.Commands.Decorators;

internal sealed class TransactionalDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
{
    private readonly ICommandHandler<TCommand> _handler;
    private readonly IModuleServiceProvider _moduleServiceProvider;
    private readonly IModuleResolver _moduleResolver;

    public TransactionalDecorator(ICommandHandler<TCommand> handler, IModuleServiceProvider moduleServiceProvider, IModuleResolver moduleResolver)
    {
        _handler = handler;
        _moduleServiceProvider = moduleServiceProvider;
        _moduleResolver = moduleResolver;
    }

    public Task HandleAsync(TCommand command, CancellationToken cancellationToken = new())
    {
        var module = _moduleResolver.ResolveForType(typeof(TCommand));
        var unitOfWork = _moduleServiceProvider.GetUnitOfWorkForModule(module);
        return unitOfWork.ExecuteAsync(command, _handler, cancellationToken);
    }
}