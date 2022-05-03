using System;
using System.Threading.Tasks;
using ModU.Abstract.Queries;
using ModU.Infrastructure.Queries;
using ModU.Infrastructure.Tests.Queries.TestData;
using NSubstitute;
using Xunit;

namespace ModU.Infrastructure.Tests.Queries;

public class QueryProcessorTests
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IQueryProcessor _queryProcessor;

    public QueryProcessorTests()
    {
        _serviceProvider = Substitute.For<IServiceProvider>();
        _queryProcessor = new QueryProcessor(_serviceProvider);
    }
    
    [Fact]
    public async Task Should_InvokeHandler_When_HandlerIsRegistered()
    {
        // Arrange
        var handlerMock = Substitute.For<IQueryHandler<TestQuery, int>>();
        _serviceProvider.GetService(typeof(IQueryHandler<TestQuery, int>)).Returns(handlerMock);
        var query = new TestQuery();

        // Act
        await Act(query);

        // Assert
        await handlerMock.Received(1).HandleAsync(query);
    }

    [Fact]
    public async Task Should_NotInvokeHandler_WhenHandlerWasNotRegistered()
    {
        // Arrange
        var query = new TestQuery();

        // Act & Assert 
        await Assert.ThrowsAsync<InvalidOperationException>(() => Act(query));
    }
    
    private Task<TResult> Act<TResult>(IQuery<TResult> query)
        => _queryProcessor.ProcessAsync(query);
    
}