using System;
using ModU.Abstract.Messaging;

namespace ModU.Infrastructure.Tests.Messaging.TestData;

public class PropertyMissingTestMessage : IMessage
{
    public Guid Id { get; set; }
}