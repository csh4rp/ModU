using System;
using ModU.Abstract.Messaging.Exceptions;
using ModU.Infrastructure.Tests.Messaging.TestData;
using Xunit;

namespace ModU.Infrastructure.Tests.Messaging;

public class MessageContractTests
{
    private readonly TestMessageContract _contract = new();
    
    [Fact]
    public void Should_ThrowException_When_PropertyIsMissing_And_AllAreRequired()
    {
        _contract.SetUpToRequireAll();
        var messageType = typeof(PropertyMissingTestMessage);
        
        var exception = Assert.Throws<ContractBrokenException>(() => Act(messageType));
        Assert.Equal(exception.Message, "Required property with name: 'Name' was not found in type: 'ModU.Infrastructure.Tests.Messaging.TestData.PropertyMissingTestMessage'.");
    }
    
    [Fact]
    public void Should_ThrowException_When_PropertyIsMissing()
    {
        _contract.SetUpToRequireName();
        var messageType = typeof(PropertyMissingTestMessage);
        
        var exception = Assert.Throws<ContractBrokenException>(() => Act(messageType));
        Assert.Equal(exception.Message, "Required property with name: 'Name' was not found in type: 'ModU.Infrastructure.Tests.Messaging.TestData.PropertyMissingTestMessage'.");
    }
    
    [Fact]
    public void Should_ThrowException_When_PropertyHasInvalidType()
    {
        _contract.SetUpToRequireName();
        var messageType = typeof(InvalidPropertyTypeTestMessage);
        
        var exception = Assert.Throws<ContractBrokenException>(() => Act(messageType));
        Assert.Equal(exception.Message, "Property: 'Name' has required type of: 'System.String', " +
                                        $"but found: 'System.Int32' in type: 'ModU.Infrastructure.Tests.Messaging.TestData.InvalidPropertyTypeTestMessage'.");
    }

    [Fact]
    public void Should_NotThrowException_WhenMessageHasAllRequiredProperties()
    {
        _contract.SetUpToRequireAll();
        var messageType = typeof(CorrectTestMessage);

        _contract.Validate(messageType);
    }

    private void Act(Type type) => _contract.Validate(type);
}