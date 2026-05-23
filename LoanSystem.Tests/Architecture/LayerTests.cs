using NetArchTest.Rules;

namespace LoanSystem.Tests.Architecture;

public class LayerTests
{
    private const string DomainNamespace = "LoanSystem.Domain";
    private const string ApplicationNamespace = "LoanSystem.Application";
    private const string InfrastructureNamespace = "LoanSystem.Infrastructure";
    private const string ApiNamespace = "LoanSystem.Api";

    [Fact]
    public void Domain_Should_Not_HaveDependencyOnOtherProjects()
    {
        var result = Types.InAssembly(typeof(LoanSystem.Domain.Primitives.BaseEntity).Assembly)
            .ShouldNot()
            .HaveDependencyOnAll(ApplicationNamespace, InfrastructureNamespace, ApiNamespace)
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Application_Should_Not_HaveDependencyOnInfrastructureAndApi()
    {
        var result = Types.InAssembly(typeof(LoanSystem.Application.Abstractions.Messaging.ICommand).Assembly)
            .ShouldNot()
            .HaveDependencyOnAll(InfrastructureNamespace, ApiNamespace)
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Infrastructure_Should_Not_HaveDependencyOnApi()
    {
        var result = Types.InAssembly(typeof(LoanSystem.Infrastructure.Database.AppDbContext).Assembly)
            .ShouldNot()
            .HaveDependencyOn(ApiNamespace)
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Controllers_Should_InheritFromApiController()
    {
        var result = Types.InAssembly(typeof(LoanSystem.Api.Controllers.ApiController).Assembly)
            .That()
            .ResideInNamespace("LoanSystem.Api.Controllers")
            .And()
            .AreClasses()
            .And()
            .AreNotAbstract()
            .Should()
            .Inherit(typeof(LoanSystem.Api.Controllers.ApiController))
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Entities_Should_InheritFromBaseEntity()
    {
        var result = Types.InAssembly(typeof(LoanSystem.Domain.Primitives.BaseEntity).Assembly)
            .That()
            .AreClasses()
            .And()
            .ResideInNamespace("LoanSystem.Domain")
            .And()
            .DoNotResideInNamespace("LoanSystem.Domain.Primitives")
            .And()
            .DoNotResideInNamespace("LoanSystem.Domain.Enums")
            .And()
            .DoNotHaveName("User")
            .Should()
            .Inherit(typeof(LoanSystem.Domain.Primitives.BaseEntity))
            .GetResult();

        Assert.True(result.IsSuccessful, $"Failing types: {string.Join(", ", result.FailingTypes?.Select(t => t.FullName) ?? Array.Empty<string>())}");
    }
}
