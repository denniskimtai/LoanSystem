using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Application.Loans.Create;
using LoanSystem.Domain.Entities.Customers;
using LoanSystem.Domain.Entities.Identity;
using LoanSystem.Domain.Entities.Loans;
using LoanSystem.Domain.Enums;
using LoanSystem.Domain.Primitives;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using Xunit;

namespace LoanSystem.Tests.Loans;

public class CreateLoanCommandHandlerTests
{
    private readonly ILoanRepository _loanRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly ILoanProductRepository _loanProductRepository;
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CreateLoanCommandHandler _handler;

    public CreateLoanCommandHandlerTests()
    {
        _loanRepository = Substitute.For<ILoanRepository>();
        _customerRepository = Substitute.For<ICustomerRepository>();
        _loanProductRepository = Substitute.For<ILoanProductRepository>();
        
        var store = Substitute.For<IUserStore<User>>();
        _userManager = Substitute.For<UserManager<User>>(store, null, null, null, null, null, null, null, null);
        
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new CreateLoanCommandHandler(
            _loanRepository,
            _customerRepository,
            _loanProductRepository,
            _userManager,
            _unitOfWork);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenCreateIsSuccessful()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var loId = Guid.NewGuid();
        var coId = Guid.NewGuid();
        var creatorId = Guid.NewGuid();
        var branchId = Guid.NewGuid();

        var customer = new Customer("Test Customer", "12345678", "0712345678", "http://photo", "Address", "Loc", "Town", "County", "Post", branchId, loId);
        var product = new LoanProduct("Test Product", 1000m, 50000m, 0.12m, 30);
        var loanOfficer = new User("lo@test.com", "Loan Officer", UserRole.LoanOfficer, branchId);
        var creditOfficer = new User("co@test.com", "Credit Officer", UserRole.LoanOfficer, branchId);

        _customerRepository.GetByIdAsync(customerId, Arg.Any<CancellationToken>()).Returns(customer);
        _loanProductRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>()).Returns(product);
        _userManager.FindByIdAsync(loId.ToString()).Returns(loanOfficer);
        _userManager.FindByIdAsync(coId.ToString()).Returns(creditOfficer);
        _loanRepository.GenerateLoanCodeAsync(Arg.Any<CancellationToken>()).Returns("LN-000001");

        var command = new CreateLoanCommand(
            customerId,
            productId,
            loId,
            coId,
            creatorId,
            15000m,
            1800m,
            LoanType.Manual);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value);
        _loanRepository.Received(1).Add(Arg.Any<Loan>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenCustomerDoesNotExist()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var command = new CreateLoanCommand(
            customerId,
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            15000m,
            1800m,
            LoanType.Manual);

        _customerRepository.GetByIdAsync(customerId, Arg.Any<CancellationToken>()).Returns((Customer?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Customer.NotFound", result.Error.Code);
        _loanRepository.DidNotReceive().Add(Arg.Any<Loan>());
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenProductDoesNotExist()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var customer = new Customer("Test Customer", "12345678", "0712345678", "http://photo", "Address", "Loc", "Town", "County", "Post", Guid.NewGuid(), Guid.NewGuid());

        _customerRepository.GetByIdAsync(customerId, Arg.Any<CancellationToken>()).Returns(customer);
        _loanProductRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>()).Returns((LoanProduct?)null);

        var command = new CreateLoanCommand(
            customerId,
            productId,
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            15000m,
            1800m,
            LoanType.Manual);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("LoanProduct.NotFound", result.Error.Code);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenPrincipalIsLessThanMinAmount()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var customer = new Customer("Test Customer", "12345678", "0712345678", "http://photo", "Address", "Loc", "Town", "County", "Post", branchId, Guid.NewGuid());
        var product = new LoanProduct("Test Product", 5000m, 50000m, 0.12m, 30);

        _customerRepository.GetByIdAsync(customerId, Arg.Any<CancellationToken>()).Returns(customer);
        _loanProductRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>()).Returns(product);

        var command = new CreateLoanCommand(
            customerId,
            productId,
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            2000m, // less than 5000m
            240m,
            LoanType.Manual);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Loan.InvalidPrincipal", result.Error.Code);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenLoanOfficerDoesNotExist()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var loId = Guid.NewGuid();
        
        var customer = new Customer("Test Customer", "12345678", "0712345678", "http://photo", "Address", "Loc", "Town", "County", "Post", branchId, loId);
        var product = new LoanProduct("Test Product", 1000m, 50000m, 0.12m, 30);

        _customerRepository.GetByIdAsync(customerId, Arg.Any<CancellationToken>()).Returns(customer);
        _loanProductRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>()).Returns(product);
        _userManager.FindByIdAsync(loId.ToString()).Returns((User?)null);

        var command = new CreateLoanCommand(
            customerId,
            productId,
            loId,
            Guid.NewGuid(),
            Guid.NewGuid(),
            15000m,
            1800m,
            LoanType.Manual);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Loan.InvalidLoanOfficer", result.Error.Code);
    }
}
