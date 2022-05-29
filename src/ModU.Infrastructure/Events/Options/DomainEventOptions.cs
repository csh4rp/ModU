﻿namespace ModU.Infrastructure.Events.Options;

public class DomainEventOptions
{
    public int MaxRetryAttempts { get; set; }
    
    public int QueueLockTime { get; set; }
}