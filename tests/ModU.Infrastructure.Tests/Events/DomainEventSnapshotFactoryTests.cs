using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Extensions.Options;
using ModU.Abstract.Contexts;
using ModU.Abstract.Security;
using ModU.Abstract.Time;
using ModU.Infrastructure.Events.Entities;
using ModU.Infrastructure.Events.Factories;
using ModU.Infrastructure.Events.Options;
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
        _factory = new DomainEventSnapshotFactory(_appContext, _clock, _hasher, _options);
    }

    [Fact]
    public void Should_CreateSnapshot_When_AllDataIsPresent()
    {
        // Arrange
        var aggregateId = Guid.NewGuid();
        var transactionId = Guid.NewGuid();
        var aggregateType = typeof(TestAggregate);
        const string hash = "HASH";
        const int maxRetryAttempts = 5;
        _clock.Now().Returns(DateTime.UtcNow);
        _hasher.ComputeMD5Hash(aggregateId + aggregateType.FullName).Returns(hash);
        _appContext.IdentityContext!.UserId.Returns(Guid.NewGuid());
        _appContext.TraceContext.TraceId.Returns(Guid.NewGuid().ToString());
        _appContext.TraceContext.SpanId.Returns(Guid.NewGuid().ToString());
        _options.Value.Returns(new DomainEventOptions { MaxRetryAttempts = maxRetryAttempts });
        var domainEvent = ADomainEvent();
        
        // Act
        var snapshot = Act(domainEvent, aggregateId, aggregateType,transactionId);

        // Asser
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
        snapshot.AggregateType.ShouldBe(aggregateType.FullName);
        snapshot.CreatedAt.ShouldBe(_clock.Now());
        snapshot.TransactionId.ShouldBe(transactionId);
        snapshot.UserId.ShouldBe(_appContext.IdentityContext.UserId);
    }

    private DomainEventSnapshot Act(TestDomainEvent @event, Guid aggregateId, Type aggregateType, Guid transactionId) 
        => _factory.Create(@event, aggregateId, aggregateType, transactionId);

    private static TestDomainEvent ADomainEvent() => new(Guid.NewGuid(), new List<int> { 1, 2 });
}