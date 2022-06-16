using System;
using ModU.Abstract.Domain;

namespace ModU.Infrastructure.Tests.Events.Domain.TestData;

public class TestAggregate : AggregateRoot
{
    public TestAggregate()
    {
        Id = Guid.NewGuid();
    }
}