namespace ModU.Abstract.Contexts;

public interface ITenantContext
{
    Guid Id { get; }
    string Name { get; }
    TenantSettings Settings { get; }
}