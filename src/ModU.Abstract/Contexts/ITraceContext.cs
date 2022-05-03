namespace ModU.Abstract.Contexts;

public interface ITraceContext
{
    string TraceId { get; }
    string SpanId { get; }
    string? IpAddress { get; }
    string? UserAgent { get; }
}