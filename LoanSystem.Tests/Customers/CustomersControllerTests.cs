using System.Security.Claims;
using LoanSystem.Api.Controllers;
using LoanSystem.Api.Customers;
using LoanSystem.Application.Customers.Guarantors;
using LoanSystem.Domain.Primitives;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace LoanSystem.Tests.Customers;

public class CustomersControllerTests
{
    private readonly ISender _sender;
    private readonly CustomersController _controller;

    public CustomersControllerTests()
    {
        _sender = Substitute.For<ISender>();
        _controller = new CustomersController(_sender);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
        }, "TestAuth"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    [Fact]
    public async Task AddGuarantor_Should_ReturnOk_WhenSuccessful()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var guarantorId = Guid.NewGuid();
        var request = new AddGuarantorRequest("John Doe", "87654321", "0722222222", 1000m, "Uncle");

        _sender.Send(Arg.Any<AddGuarantorCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success(guarantorId));

        // Act
        var result = await _controller.AddGuarantor(customerId, request, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(guarantorId, okResult.Value);
    }
}
