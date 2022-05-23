using ModU.Abstract.Time;

namespace ModU.Infrastructure.Time;

internal sealed class UtcClock : IClock
{
    public DateTime Now() => DateTime.UtcNow;
}