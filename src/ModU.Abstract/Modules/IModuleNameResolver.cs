namespace ModU.Abstract.Modules;

public interface IModuleNameResolver
{
    string Resolve(string typeFullName);
}