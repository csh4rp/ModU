using System;
using ModU.Abstract.Messaging.Contracts;
using ModU.Infrastructure.Tests.Messaging.TestData;
using Xunit;

namespace ModU.Infrastructure.Tests.Messaging;

public class MessageContractTests
{
    private readonly TestMessageContract _contract = new();
    
    [Fact]
    public void Should_ReturnErrors_When_PropertyIsMissing_And_AllAreRequired()
    {
        // Arrange
        _contract.RequireAll();
        var messageType = typeof(PropertyMissingTestMessage);

        // Act
        var result = Act(messageType);
        
        // Assert
        Assert.False(result.IsValid);
        Assert.Equal(1, result.Errors.Count);
        Assert.Equal(nameof(TestMessage.Name), result.Errors[0].PropertyName);
        Assert.Equal("Required property with name: 'Name' was not found in type: " +
                     "'ModU.Infrastructure.Tests.Messaging.TestData.PropertyMissingTestMessage'.", result.Errors[0].ErrorMessage);
    }
    
    [Fact]
    public void Should_ReturnErrors_When_PropertyIsMissing()
    {
        // Arrange
        _contract.Require(x => x.Name);
        var messageType = typeof(PropertyMissingTestMessage);

        // Act
        var result = Act(messageType);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal(1, result.Errors.Count);
        Assert.Equal(nameof(TestMessage.Name), result.Errors[0].PropertyName);
        Assert.Equal("Required property with name: 'Name' was not found in type: " +
                     "'ModU.Infrastructure.Tests.Messaging.TestData.PropertyMissingTestMessage'.", result.Errors[0].ErrorMessage);
    }
    
    [Fact]
    public void Should_ReturnErrors_When_PropertyHasInvalidType()
    {
        // Arrange
        _contract.Require(x => x.Name);
        var messageType = typeof(InvalidPropertyTypeTestMessage);

        // Act
        var result = Act(messageType);
        
        // Assert
        Assert.False(result.IsValid);
        Assert.Equal(1, result.Errors.Count);
        Assert.Equal(nameof(TestMessage.Name), result.Errors[0].PropertyName);
        Assert.Equal("Property: 'Name' has required type of: 'System.String', but found: 'System.Int32' in type: " +
                     "'ModU.Infrastructure.Tests.Messaging.TestData.InvalidPropertyTypeTestMessage'.",result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Should_NotReturnErrors_When_MessageHasAllRequiredProperties()
    {
        // Arrange
        _contract.RequireAll();
        var messageType = typeof(CorrectTestMessage);

        // Act
        var result = Act(messageType);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
    
    [Fact]
    public void Should_NotReturnErrors_When_PropertyWithDifferentTypeIsIgnored()
    {
        // Arrange
        _contract.Require(x => x.Id).Ignore(x => x.Name);
        var messageType = typeof(InvalidPropertyTypeTestMessage);

        // Act
        var result = Act(messageType);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
    
    [Fact]
    public void Should_OverridePropertyContract_When_PropertyIsSpecifiedTwice()
    {
        // Arrange
        _contract.Require(x => x.Id).Require(x => x.Name).Ignore(x => x.Name);
        var messageType = typeof(InvalidPropertyTypeTestMessage);

        // Act
        var result = Act(messageType);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
    
    [Fact]
    public void Should_ThrowInvalidOperationException_When_ExpressionIsNotMemberExpression()
    {
        // Arrange
        void Act() => _contract.Require(x => true);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(Act);
        Assert.Equal("A expression must be a member expression.", exception.Message);
    }

    private MessageContractValidationResult Act(Type type) => _contract.Validate(type);
}