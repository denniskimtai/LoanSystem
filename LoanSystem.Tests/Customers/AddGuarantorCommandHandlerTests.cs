using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Application.Customers.Guarantors;
using LoanSystem.Domain.Entities.Customers;
using LoanSystem.Domain.Primitives;
using NSubstitute;
using Xunit;

namespace LoanSystem.Tests.Customers;

public class AddGuarantorCommandHandlerTests
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly AddGuarantorCommandHandler _handler;

    public AddGuarantorCommandHandlerTests()
    {
        _customerRepository = Substitute.For<ICustomerRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new AddGuarantorCommandHandler(_customerRepository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenAddIsSuccessful()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var customer = new Customer(
            "Dennis Tai",
            "12345678",
            "0712345678",
            "http://photo.url",
            "Nairobi",
            "1,2",
            "Nairobi",
            "Nairobi",
            "P.O Box 1",
            Guid.NewGuid(),
            Guid.NewGuid());

        _customerRepository.GetByIdAsync(customerId, Arg.Any<CancellationToken>())
            .Returns(customer);

        var command = new AddGuarantorCommand(
            customerId,
            "John Doe",
            "87654321",
            "0722222222",
            1000m,
            "Uncle");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(customer.Guarantors);
        var guarantor = customer.Guarantors.First();
        Assert.Equal("John Doe", guarantor.Name);
        Assert.Equal("87654321", guarantor.IdNumber);
        Assert.Equal("0722222222", guarantor.Phone);
        Assert.Equal(1000m, guarantor.AmountGuaranteed);
        Assert.Equal("Uncle", guarantor.Relationship);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenCustomerDoesNotExist()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        _customerRepository.GetByIdAsync(customerId, Arg.Any<CancellationToken>())
            .Returns((Customer?)null);

        var command = new AddGuarantorCommand(
            customerId,
            "John Doe",
            "87654321",
            "0722222222",
            1000m,
            "Uncle");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Customer.NotFound", result.Error.Code);
    }
}
