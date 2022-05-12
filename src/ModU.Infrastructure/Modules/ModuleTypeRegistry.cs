namespace ModU.Infrastructure.Modules;

public class ModuleTypeRegistry
{
    private readonly Dictionary<string, Type> _unitOfWorkTypes = new();
    private readonly Dictionary<string, Type> _dbContextTypes = new();
    private readonly ModuleNameResolver _moduleNameResolver = new();
    
    public static ModuleTypeRegistry Instance { get; } = new();

    private ModuleTypeRegistry()
    {
    }
    
    public void AddUnitOfWork(Type unitOfWorkType)
    {
        var moduleName = _moduleNameResolver.Resolve(unitOfWorkType.FullName!);
        _unitOfWorkTypes.Add(moduleName, unitOfWorkType);
    }

    public void AddDbContext(Type dbContextType)
    {
        var moduleName = _moduleNameResolver.Resolve(dbContextType.FullName!);
        _dbContextTypes.Add(moduleName, dbContextType);
    }

    public Type GetUnitOfWorkType(string moduleName) => _unitOfWorkTypes[moduleName];

    public Type GetContextType(string moduleName) => _dbContextTypes[moduleName];
}