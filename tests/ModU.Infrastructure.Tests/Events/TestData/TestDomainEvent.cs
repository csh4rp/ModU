using System;
using System.Collections.Generic;
using ModU.Abstract.Domain;

namespace ModU.Infrastructure.Tests.Events.TestData;

[DomainEvent("test_domain_event")]
public class TestDomainEvent : IDomainEvent
{
    private TestDomainEvent()
    {
    }
    
    public TestDomainEvent(Guid id, List<int> ids)
    {
        Id = id;
        Ids = ids;
    }

    public Guid Id { get; private set; }
    public List<int> Ids { get; private set; } = null!;
}