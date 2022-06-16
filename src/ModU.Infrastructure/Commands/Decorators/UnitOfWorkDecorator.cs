using ModU.Abstract.Commands;
using ModU.Infrastructure.Database;
using ModU.Infrastructure.Events.Domain.Services;

namespace ModU.Infrastructure.Commands.Decorators;

internal sealed class UnitOfWorkDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
{
    private readonly BaseDbContext _dbContext;
    private readonly ICommandHandler<TCommand> _handler;
    private readonly IDomainEventAccessor _domainEventAccessor;

    public UnitOfWorkDecorator(BaseDbContext dbContext, ICommandHandler<TCommand> handler, IDomainEventAccessor domainEventAccessor)
    {
        _dbContext = dbContext;
        _handler = handler;
        _domainEventAccessor = domainEventAccessor;
    }

    public async Task HandleAsync(TCommand command, CancellationToken cancellationToken = new())
    {
        await _handler.HandleAsync(command, cancellationToken);
        var domainEvents = _domainEventAccessor.GetEvents();
        
        _dbContext.AddRange(domainEvents);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}