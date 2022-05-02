using System.Linq.Expressions;
using System.Reflection;
using ModU.Abstract.Messaging.Exceptions;

namespace ModU.Abstract.Messaging;

public abstract class MessageContract<TMessage> : IMessageContract<TMessage> where TMessage : IMessage
{
    private readonly List<PropertyContract> _propertyContracts = new();

    protected void RequireAll()
    {
        var properties = typeof(TMessage).GetProperties(BindingFlags.Instance | BindingFlags.Public);
        foreach (var propertyInfo in properties)
        {
            _propertyContracts.Add(new PropertyContract(propertyInfo.Name, propertyInfo.PropertyType, false));
        }
    }

    protected void Require<T>(Expression<Func<TMessage, T>> propertyExpression)
    {
        if (propertyExpression.Body is not MemberExpression exp)
        {
            throw new InvalidOperationException("A expression must be a member expression");
        }

        var propertyInfo = (PropertyInfo) exp.Member;
        _propertyContracts.Add(new PropertyContract(propertyInfo.Name, propertyInfo.PropertyType, false));
    }

    protected void Ignore<T>(Expression<Func<TMessage, T>> propertyExpression)
    {
        if (propertyExpression.Body is not MemberExpression exp)
        {
            throw new InvalidOperationException("A expression must be a member expression");
        }

        var propertyInfo = (PropertyInfo) exp.Member;
        _propertyContracts.Add(new PropertyContract(propertyInfo.Name, propertyInfo.PropertyType, true));
    }
    
    public void Validate(Type type)
    {
        var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        foreach (var propertyContract in _propertyContracts)
        {
            if (propertyContract.Ignore)
            {
                continue;
            }
            
            var property = properties.FirstOrDefault(p => p.Name == propertyContract.PropertyName);
            if (property is null)
            {
                throw new ContractBrokenException(
                    $"Required property with name: '{propertyContract.PropertyName}' was not found in type: '{type.FullName}'.");
            }

            if (propertyContract.PropertyType != property.PropertyType)
            {
                throw new ContractBrokenException(
                    $"Property: '{propertyContract.PropertyName}' has required type of: '{propertyContract.PropertyType.FullName}', " +
                    $"but found: '{property.PropertyType}' in type: '{type.FullName}'.");
            }
        }

    }
}