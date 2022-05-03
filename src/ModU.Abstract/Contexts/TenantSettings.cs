namespace ModU.Abstract.Contexts;

public sealed class TenantSettings
{
    public TenantSettings(string connectionString) => ConnectionString = connectionString;

    public string ConnectionString { get; }
}