using System;
using System.Threading.Tasks;
using ModU.Abstract.Commands;
using ModU.Infrastructure.Commands;
using ModU.Infrastructure.Tests.Commands.TestData;
using NSubstitute;
using Xunit;

namespace ModU.Infrastructure.Tests.Commands;

public class CommandProcessorTests
{
    private readonly ICommandProcessor _commandProcessor;
    private readonly IServiceProvider _serviceProvider;

    public CommandProcessorTests()
    {
        _serviceProvider = Substitute.For<IServiceProvider>();
        _commandProcessor = new CommandProcessor(_serviceProvider);
    }

    [Fact]
    public async Task Should_InvokeHandler_When_HandlerIsRegistered()
    {
        // Arrange
        var handlerMock = Substitute.For<ICommandHandler<TestCommand>>();
        _serviceProvider.GetService(typeof(ICommandHandler<TestCommand>)).Returns(handlerMock);
        var command = new TestCommand();

        // Act
        await Act(command);

        // Assert
        await handlerMock.Received(1).HandleAsync(command);
    }

    [Fact]
    public async Task Should_InvokeHandlerWithResult_When_HandlerIsRegistered()
    {
        // Arrange
        var command = new TestGenericCommand();
        const int expectedResult = 1;
        var handlerMock = Substitute.For<ICommandHandler<ICommand<int>, int>>();
        handlerMock.HandleAsync(command).Returns(Task.FromResult(expectedResult));
        _serviceProvider.GetService(typeof(ICommandHandler<TestGenericCommand, int>)).Returns(handlerMock);

        // Act
        var result = await Act(command);

        // Assert
        Assert.Equal(expectedResult, result);
        await handlerMock.Received(1).HandleAsync(command);
    }

    [Fact]
    public async Task Should_NotInvokeHandler_WhenHandlerWasNotRegistered()
    {
        // Arrange
        var command = new TestCommand();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => Act(command));
    }
    
    [Fact]
    public async Task Should_NotInvokeHandlerWithResult_WhenHandlerWasNotRegistered()
    {
        // Arrange
        var command = new TestGenericCommand();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => Act(command));
    }

    private Task Act(ICommand command) => _commandProcessor.ProcessAsync(command);
    
    private Task<TResult> Act<TResult>(ICommand<TResult> command) => _commandProcessor.ProcessAsync(command);

}