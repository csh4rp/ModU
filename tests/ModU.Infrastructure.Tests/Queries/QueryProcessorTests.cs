using System;
using System.Threading.Tasks;
using ModU.Abstract.Queries;
using ModU.Infrastructure.Queries;
using ModU.Infrastructure.Tests.Queries.Fakes;
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
        var handlerMock = Substitute.For<IQueryHandler<FakeQuery, int>>();
        _serviceProvider.GetService(typeof(IQueryHandler<FakeQuery, int>)).Returns(handlerMock);
        var query = new FakeQuery();

        await Act(query);

        await handlerMock.Received(1).HandleAsync(query);
    }

    [Fact]
    public async Task Should_NotInvokeHandler_WhenHandlerWasNotRegistered()
    {
        var query = new FakeQuery();

        await Assert.ThrowsAsync<InvalidOperationException>(() => Act(query));
    }
    
    private Task<TResult> Act<TResult>(IQuery<TResult> query)
        => _queryProcessor.ProcessAsync(query);
    
}