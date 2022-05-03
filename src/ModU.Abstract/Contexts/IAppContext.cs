namespace ModU.Abstract.Contexts;

public interface IAppContext
{
    IIdentityContext? IdentityContext { get; }
    ITraceContext TraceContext { get; }
    ITenantContext TenantContext { get; }
}