using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Extensions.Options;
using ModU.Abstract.Contexts;
using ModU.Abstract.Domain;
using ModU.Abstract.Security;
using ModU.Abstract.Time;
using ModU.Infrastructure.Events.Domain.Entities;
using ModU.Infrastructure.Events.Domain.Factories;
using ModU.Infrastructure.Events.Domain.Options;
using ModU.Infrastructure.Tests.Events.Domain.TestData;
using ModU.Infrastructure.Tests.Events.TestData;
using NSubstitute;
using Shouldly;
using Xunit;

namespace ModU.Infrastructure.Tests.Events;

public class DomainEventSnapshotFactoryTests
{
    private readonly IClock _clock;
    private readonly IHasher _hasher;
    private readonly IAppContext _appContext;
    private readonly IOptions<DomainEventOptions> _options;
    private readonly IDomainEventSnapshotFactory _factory;
    
    public DomainEventSnapshotFactoryTests()
    {
        _clock = Substitute.For<IClock>();
        _hasher = Substitute.For<IHasher>();
        _appContext = Substitute.For<IAppContext>();
        _options = Substitute.For<IOptions<DomainEventOptions>>();
        _factory = new DomainEventSnapshotFactory(_appContext, _clock, _hasher);
    }

    [Fact]
    public void Should_CreateSnapshot_When_AllDataIsPresent()
    {
        // Arrange
        var aggregateId = Guid.NewGuid();
        var transactionId = Guid.NewGuid();
        var aggregate = new TestAggregate();
        const string hash = "HASH";
        const int maxRetryAttempts = 5;
        _clock.Now().Returns(DateTime.UtcNow);
        _hasher.ComputeMD5Hash(aggregateId + aggregate.GetType().FullName).Returns(hash);
        _appContext.IdentityContext!.UserId.Returns(Guid.NewGuid());
        _appContext.TraceContext.TraceId.Returns(Guid.NewGuid().ToString());
        _appContext.TraceContext.SpanId.Returns(Guid.NewGuid().ToString());
        _options.Value.Returns(new DomainEventOptions {  });
        var domainEvent = ADomainEvent();
        
        // Act
        var snapshot = Act(domainEvent, aggregate, transactionId);

        // Assert
        snapshot.ShouldNotBeNull();
        snapshot.Id.ShouldNotBe(Guid.Empty);
        snapshot.Name.ShouldBe("test_domain_event");
        snapshot.Type.ShouldBe(typeof(TestDomainEvent).FullName);
        snapshot.Data.RootElement.ToString().ShouldBeEquivalentTo(JsonSerializer.Serialize(domainEvent));
        
        snapshot.FailedAttempts.ShouldBe(0);
        snapshot.MaxAttempts.ShouldBe(maxRetryAttempts);
        snapshot.DeliveredAt.ShouldBeNull();
        snapshot.FailedAt.ShouldBeNull();
        snapshot.NextAttemptAt.ShouldBeNull();

        snapshot.Queue.ShouldBe(hash);
        snapshot.TraceId.ShouldBe(_appContext.TraceContext.TraceId);
        snapshot.SpanId.ShouldBe(_appContext.TraceContext.SpanId);
        snapshot.AggregateId.ShouldBe(aggregateId);
        snapshot.AggregateType.ShouldBe(aggregate.GetType().FullName!);
        snapshot.CreatedAt.ShouldBe(_clock.Now());
        snapshot.TransactionId.ShouldBe(transactionId);
        snapshot.UserId.ShouldBe(_appContext.IdentityContext.UserId);
    }

    private DomainEventSnapshot Act(TestDomainEvent @event, IAggregateRoot aggregateRoot, Guid transactionId) 
        => _factory.Create(@event, aggregateRoot, transactionId);

    private static TestDomainEvent ADomainEvent() => new(Guid.NewGuid(), new List<int> { 1, 2 });
}