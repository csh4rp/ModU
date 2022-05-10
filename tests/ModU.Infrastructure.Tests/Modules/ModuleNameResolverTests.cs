using System;
using ModU.Infrastructure.Modules;
using Xunit;

namespace ModU.Infrastructure.Tests.Modules;

public class ModuleNameResolverTests
{
    private static readonly ModuleNameResolver Resolver = new();
    
    [Theory]
    [InlineData("ModU.Modules.Resources.Application.Command", "Resources")]
    [InlineData("ModU.Modules.Identity.Domain.BaseEntity", "Identity")]
    [InlineData("ModU.Modules.Products", "Products")]
    public void Should_ResolveModuleName_When_NamespaceMatchesSchema(string typeFullName, string moduleName)
    {
        var resolvedModuleName = Act(typeFullName);
        
        Assert.Equal(moduleName, resolvedModuleName);
    }
    
    [Theory]
    [InlineData("ModU.Modules")]
    [InlineData("ModU.Modules.")]
    public void Should_ThrowException_When_NamespaceDoesNotMatchSchema(string typeFullName)
    {
        var exception = Assert.Throws<ArgumentException>(() => Act(typeFullName));
        Assert.Equal("typeFullName", exception.ParamName);
    }

    private static string Act(string typeFullName) => Resolver.Resolve(typeFullName);
}