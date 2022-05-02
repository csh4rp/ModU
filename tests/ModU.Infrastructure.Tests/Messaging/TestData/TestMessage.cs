using System;
using ModU.Abstract.Messaging;

namespace ModU.Infrastructure.Tests.Messaging.TestData;

public class TestMessage : IMessage
{
    public Guid Id { get; set; }
    
    public string? Name { get; set; }
}