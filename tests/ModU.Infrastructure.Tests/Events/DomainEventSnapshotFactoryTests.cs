using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Text.Json;
using Microsoft.Extensions.Options;
using ModU.Abstract.Contexts;
using ModU.Abstract.Security;
using ModU.Abstract.Time;
using ModU.Infrastructure.Events.Factories;
using ModU.Infrastructure.Events.Model;
using ModU.Infrastructure.Events.Options;
using ModU.Infrastructure.Security;
using ModU.Infrastructure.Tests.Events.TestData;
using ModU.Infrastructure.Time;
using NSubstitute;
using Shouldly;
using Xunit;

namespace ModU.Infrastructure.Tests.Events;

public class DomainEventSnapshotFactoryTests
{
    private readonly IAppContext _appContext;
    private readonly IOptions<DomainEventOptions> _options;
    private readonly IDomainEventSnapshotFactory _factory;
    
    public DomainEventSnapshotFactoryTests()
    {
        var clock = new UtcClock();
        var hasher = new Hasher();
        _appContext = Substitute.For<IAppContext>();
        _options = Substitute.For<IOptions<DomainEventOptions>>();
        _factory = new DomainEventSnapshotFactory(_appContext, clock, hasher, _options);
    }

    [Fact]
    public void Should_CreateSnapshot_When_AllDataIsPresent()
    {
        // Arrange
        var aggregateId = Guid.NewGuid();
        var transactionId = Guid.NewGuid();
        var aggregateType = typeof(TestAggregate);
        const int maxRetryAttempts = 5;
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
        snapshot.Content.Name.ShouldBe("test_domain_event");
        snapshot.Content.Type.ShouldBe(typeof(TestDomainEvent).FullName);
        snapshot.Content.Data.RootElement.ToString().ShouldBeEquivalentTo(JsonSerializer.Serialize(domainEvent));
        
        snapshot.DeliveryInfo.AttemptNumber.ShouldBe(0);
        snapshot.DeliveryInfo.MaxAttempts.ShouldBe(maxRetryAttempts);
        snapshot.DeliveryInfo.DeliveredAt.ShouldBeNull();
        snapshot.DeliveryInfo.FailedAt.ShouldBeNull();
        snapshot.DeliveryInfo.NextAttemptAt.ShouldBeNull();

        snapshot.MetaData.Queue.ShouldNotBeEmpty();
        snapshot.MetaData.TraceId.ShouldBe(_appContext.TraceContext.TraceId);
        snapshot.MetaData.SpanId.ShouldBe(_appContext.TraceContext.SpanId);
        snapshot.MetaData.AggregateId.ShouldBe(aggregateId);
        snapshot.MetaData.AggregateType.ShouldBe(aggregateType.FullName);
        snapshot.MetaData.CreatedAt.ShouldNotBe(default);
        snapshot.MetaData.TransactionId.ShouldBe(transactionId);
        snapshot.MetaData.UserId.ShouldBe(_appContext.IdentityContext.UserId);
    }

    private DomainEventSnapshot Act(TestDomainEvent @event, Guid aggregateId, Type aggregateType, Guid transactionId)
    {
        var snapshot = _factory.Create(@event, aggregateId, aggregateType, transactionId);
        return snapshot;
    }

    private static TestDomainEvent ADomainEvent() => new(Guid.NewGuid(), new List<int> { 1, 2 });
}