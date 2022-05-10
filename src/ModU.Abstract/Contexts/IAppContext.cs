namespace ModU.Abstract.Contexts;

public interface IAppContext
{
    IIdentityContext? IdentityContext { get; }
    ITenantContext TenantContext { get; }
}