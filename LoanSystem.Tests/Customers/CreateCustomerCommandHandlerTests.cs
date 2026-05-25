using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Application.Customers.Create;
using LoanSystem.Domain.Entities.Customers;
using LoanSystem.Domain.Entities.Identity;
using LoanSystem.Domain.Enums;
using LoanSystem.Domain.Primitives;
using NSubstitute;

namespace LoanSystem.Tests.Customers;

public class CreateCustomerCommandHandlerTests
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CreateCustomerCommandHandler _handler;

    public CreateCustomerCommandHandlerTests()
    {
        _customerRepository = Substitute.For<ICustomerRepository>();
        _branchRepository = Substitute.For<IBranchRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new CreateCustomerCommandHandler(_customerRepository, _branchRepository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenCreateIsSuccessful()
    {
        // Arrange
        var branchId = Guid.NewGuid();
        var creatorId = Guid.NewGuid();
        var branch = new Branch("Nairobi", "CBD");

        _branchRepository.GetByIdAsync(branchId, Arg.Any<CancellationToken>())
            .Returns(branch);

        _customerRepository.ExistsByNationalIdAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(false);

        _customerRepository.ExistsByPhoneAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var command = new CreateCustomerCommand(
            "Dennis Tai",
            "12345678",
            "0712345678",
            "http://photo.url",
            "Nairobi, Kenya",
            "1.29,36.82",
            "Nairobi",
            "Nairobi County",
            "P.O Box 100",
            branchId,
            creatorId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value);
        _customerRepository.Received(1).Add(Arg.Any<Customer>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenBranchDoesNotExist()
    {
        // Arrange
        var branchId = Guid.NewGuid();
        var creatorId = Guid.NewGuid();

        _branchRepository.GetByIdAsync(branchId, Arg.Any<CancellationToken>())
            .Returns((Branch?)null);

        var command = new CreateCustomerCommand(
            "Dennis Tai",
            "12345678",
            "0712345678",
            "http://photo.url",
            "Nairobi, Kenya",
            "1.29,36.82",
            "Nairobi",
            "Nairobi County",
            "P.O Box 100",
            branchId,
            creatorId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Customer.BranchNotFound", result.Error.Code);
        _customerRepository.DidNotReceive().Add(Arg.Any<Customer>());
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenNationalIdIsDuplicate()
    {
        // Arrange
        var branchId = Guid.NewGuid();
        var creatorId = Guid.NewGuid();
        var branch = new Branch("Nairobi", "CBD");

        _branchRepository.GetByIdAsync(branchId, Arg.Any<CancellationToken>())
            .Returns(branch);

        _customerRepository.ExistsByNationalIdAsync("12345678", Arg.Any<CancellationToken>())
            .Returns(true);

        var command = new CreateCustomerCommand(
            "Dennis Tai",
            "12345678",
            "0712345678",
            "http://photo.url",
            "Nairobi, Kenya",
            "1.29,36.82",
            "Nairobi",
            "Nairobi County",
            "P.O Box 100",
            branchId,
            creatorId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Customer.DuplicateNationalId", result.Error.Code);
    }
}
