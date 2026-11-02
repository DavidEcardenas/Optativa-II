using System.Reflection;
using LastMile.Domain.Entregas;
using NetArchTest.Rules;
using Xunit;

namespace LastMile.Architecture.Tests;

public class LayerDependencyTests
{
    private static readonly Assembly DomainAssembly =
        typeof(Delivery).Assembly;

    private static readonly Assembly ApplicationAssembly =
        typeof(LastMile.Application.Entregas.AssignCourierUseCase).Assembly;

    [Fact]
    public void Domain_ShouldNotDependOn_Application()
    {
        var result = Types.InAssembly(DomainAssembly)
            .ShouldNot()
            .HaveDependencyOn(ApplicationAssembly.GetName().Name)
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Domain_ShouldNotDependOn_Infrastructure()
    {
        var result = Types.InAssembly(DomainAssembly)
            .ShouldNot()
            .HaveDependencyOn("LastMile.Infrastructure")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Application_ShouldNotDependOn_Infrastructure()
    {
        var result = Types.InAssembly(ApplicationAssembly)
            .ShouldNot()
            .HaveDependencyOn("LastMile.Infrastructure")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }
}