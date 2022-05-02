using System;
using ModU.Abstract.Messaging;

namespace ModU.Infrastructure.Tests.Messaging.TestData;

public class InvalidPropertyTypeTestMessage : IMessage
{
    public Guid Id { get; set; }
    
    public int Name { get; set; }
}