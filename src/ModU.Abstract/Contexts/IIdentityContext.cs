namespace ModU.Abstract.Contexts;

public interface IIdentityContext
{
    Guid UserId { get; }
    string UserName { get; }
    IReadOnlyCollection<string> Roles { get; }
}