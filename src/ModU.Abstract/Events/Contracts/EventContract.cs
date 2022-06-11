using System.Linq.Expressions;
using System.Reflection;
using ModU.Abstract.Events.Common;

namespace ModU.Abstract.Events.Contracts;

public abstract class EventContract<TEvent> : IEventContract<TEvent> where TEvent : IEvent
{
    private readonly List<PropertyContract> _propertyContracts = new();

    public EventContract<TEvent> RequireAll()
    {
        var properties = typeof(TEvent).GetProperties(BindingFlags.Instance | BindingFlags.Public);
        foreach (var propertyInfo in properties)
        {
            var contract = new PropertyContract(propertyInfo.Name, propertyInfo.PropertyType, false);
            var existingContract = _propertyContracts.FirstOrDefault(c => c.PropertyName == propertyInfo.Name);
            if (existingContract is not null)
            {
                _propertyContracts.Remove(existingContract);
            }

            _propertyContracts.Add(contract);
        }

        return this;
    }

    public EventContract<TEvent> Require<T>(Expression<Func<TEvent, T>> propertyExpression)
    {
        if (propertyExpression.Body is not MemberExpression exp)
        {
            throw new InvalidOperationException("A expression must be a member expression.");
        }

        var propertyInfo = (PropertyInfo) exp.Member;
        var contract = new PropertyContract(propertyInfo.Name, propertyInfo.PropertyType, false);
        var existingContract = _propertyContracts.FirstOrDefault(c => c.PropertyName == propertyInfo.Name);
        if (existingContract is not null)
        {
            _propertyContracts.Remove(existingContract);
        }

        _propertyContracts.Add(contract);

        return this;
    }

    public EventContract<TEvent> Ignore<T>(Expression<Func<TEvent, T>> propertyExpression)
    {
        if (propertyExpression.Body is not MemberExpression exp)
        {
            throw new InvalidOperationException("A expression must be a member expression.");
        }

        var propertyInfo = (PropertyInfo) exp.Member;
        var contract = new PropertyContract(propertyInfo.Name, propertyInfo.PropertyType, true);
        var existingContract = _propertyContracts.FirstOrDefault(c => c.PropertyName == propertyInfo.Name);
        if (existingContract is not null)
        {
            _propertyContracts.Remove(existingContract);
        }
        
        _propertyContracts.Add(contract);

        return this;
    }
    
    public EventContractValidationResult Validate(Type type)
    {
        var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        var errors = new List<EventContractValidationError>();
        foreach (var propertyContract in _propertyContracts)
        {
            if (propertyContract.Ignore)
            {
                continue;
            }
            
            var property = properties.FirstOrDefault(p => p.Name == propertyContract.PropertyName);
            if (property is null)
            {
                var error = new EventContractValidationError(propertyContract.PropertyName,
                    $"Required property with name: '{propertyContract.PropertyName}' was not found in type: '{type.FullName}'.");
                errors.Add(error);
                continue;
            }

            if (propertyContract.PropertyType != property.PropertyType)
            {
                var error = new EventContractValidationError(propertyContract.PropertyName,
                    $"Property: '{propertyContract.PropertyName}' has required type of: '{propertyContract.PropertyType.FullName}', " +
                    $"but found: '{property.PropertyType}' in type: '{type.FullName}'.");
                errors.Add(error);
            }
        }

        return errors.Any() ? EventContractValidationResult.Invalid(errors) : EventContractValidationResult.Valid();

    }
}