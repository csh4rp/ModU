using Microsoft.Extensions.DependencyInjection;
using ModU.Abstract.Commands;

namespace ModU.Infrastructure.Commands.Decorators;

internal sealed class TransactionalDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
{
    private readonly ICommandHandler<TCommand> _handler;
    private readonly UnitOfWorkTypeRegistry _unitOfWorkTypeRegistry;
    private readonly IServiceProvider _serviceProvider;

    public TransactionalDecorator(ICommandHandler<TCommand> handler, UnitOfWorkTypeRegistry unitOfWorkTypeRegistry, IServiceProvider serviceProvider)
    {
        _handler = handler;
        _unitOfWorkTypeRegistry = unitOfWorkTypeRegistry;
        _serviceProvider = serviceProvider;
    }

    public async Task HandleAsync(TCommand command, CancellationToken cancellationToken = new())
    {
        if (!_unitOfWorkTypeRegistry.TryGetContextType(typeof(ICommandHandler<TCommand>), out var type))
        {
            throw new InvalidOperationException(
                $"Unit of work is not registered for type: '{typeof(ICommandHandler<TCommand>)}'");
        }

        var context = _serviceProvider.GetRequiredService(typeof(UnitOfWork<>).MakeGenericType(type));
    }
}