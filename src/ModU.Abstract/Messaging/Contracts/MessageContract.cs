using System.Linq.Expressions;
using System.Reflection;
using ModU.Abstract.Messaging.Exceptions;

namespace ModU.Abstract.Messaging.Contracts;

public abstract class MessageContract<TMessage> : IMessageContract<TMessage> where TMessage : IMessage
{
    private readonly List<PropertyContract> _propertyContracts = new();

    public MessageContract<TMessage> RequireAll()
    {
        var properties = typeof(TMessage).GetProperties(BindingFlags.Instance | BindingFlags.Public);
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

    public MessageContract<TMessage> Require<T>(Expression<Func<TMessage, T>> propertyExpression)
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

    public MessageContract<TMessage> Ignore<T>(Expression<Func<TMessage, T>> propertyExpression)
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
    
    public MessageContractValidationResult Validate(Type type)
    {
        var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        var errors = new List<MessageContractValidationError>();
        foreach (var propertyContract in _propertyContracts)
        {
            if (propertyContract.Ignore)
            {
                continue;
            }
            
            var property = properties.FirstOrDefault(p => p.Name == propertyContract.PropertyName);
            if (property is null)
            {
                var error = new MessageContractValidationError(propertyContract.PropertyName,
                    $"Required property with name: '{propertyContract.PropertyName}' was not found in type: '{type.FullName}'.");
                errors.Add(error);
                continue;
            }

            if (propertyContract.PropertyType != property.PropertyType)
            {
                var error = new MessageContractValidationError(propertyContract.PropertyName,
                    $"Property: '{propertyContract.PropertyName}' has required type of: '{propertyContract.PropertyType.FullName}', " +
                    $"but found: '{property.PropertyType}' in type: '{type.FullName}'.");
                errors.Add(error);
            }
        }

        return errors.Any() ? MessageContractValidationResult.Invalid(errors) : MessageContractValidationResult.Valid();

    }
}