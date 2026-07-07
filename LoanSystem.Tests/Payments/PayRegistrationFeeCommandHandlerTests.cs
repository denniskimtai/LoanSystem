using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Application.Customers.PayRegistrationFee;
using LoanSystem.Domain.Entities.Customers;
using LoanSystem.Domain.Entities.Identity;
using LoanSystem.Domain.Entities.Payments;
using LoanSystem.Domain.Enums;
using LoanSystem.Domain.Primitives;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using Xunit;

namespace LoanSystem.Tests.Payments;

public class PayRegistrationFeeCommandHandlerTests
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly PayRegistrationFeeCommandHandler _handler;

    public PayRegistrationFeeCommandHandlerTests()
    {
        _customerRepository = Substitute.For<ICustomerRepository>();
        _paymentRepository = Substitute.For<IPaymentRepository>();
        
        var store = Substitute.For<IUserStore<User>>();
        _userManager = Substitute.For<UserManager<User>>(store, null, null, null, null, null, null, null, null);
        
        _unitOfWork = Substitute.For<IUnitOfWork>();
        
        _handler = new PayRegistrationFeeCommandHandler(
            _customerRepository,
            _paymentRepository,
            _userManager,
            _unitOfWork);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenPaymentIsSuccessful()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var recordedById = Guid.NewGuid();
        var branchId = Guid.NewGuid();

        var customer = new Customer("Test Customer", "12345678", "0712345678", "photo.png", "Address", "Geo", "Town", "County", "Postal", branchId, recordedById);
        // Starts as Lead and RegistrationFeePaid = false

        var user = new User("admin@test.com", "Admin User", UserRole.Admin, branchId);

        _customerRepository.GetByIdAsync(customerId, Arg.Any<CancellationToken>()).Returns(customer);
        _userManager.FindByIdAsync(recordedById.ToString()).Returns(user);

        var command = new PayRegistrationFeeCommand(
            customerId,
            500m,
            "TX12345",
            "REF98765",
            PaymentMethod.MobilePayment,
            recordedById);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(customer.RegistrationFeePaid);
        Assert.Equal(CustomerStatus.Active, customer.Status);
        _paymentRepository.Received(1).Add(Arg.Any<Payment>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenCustomerDoesNotExist()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        _customerRepository.GetByIdAsync(customerId, Arg.Any<CancellationToken>()).Returns((Customer?)null);

        var command = new PayRegistrationFeeCommand(
            customerId,
            500m,
            "TX12345",
            "REF98765",
            PaymentMethod.MobilePayment,
            Guid.NewGuid());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Customer.NotFound", result.Error.Code);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenFeeAlreadyPaid()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var recordedById = Guid.NewGuid();
        var branchId = Guid.NewGuid();

        var customer = new Customer("Test Customer", "12345678", "0712345678", "photo.png", "Address", "Geo", "Town", "County", "Postal", branchId, recordedById);
        customer.PayRegistrationFee(); // Already paid!

        var user = new User("admin@test.com", "Admin User", UserRole.Admin, branchId);

        _customerRepository.GetByIdAsync(customerId, Arg.Any<CancellationToken>()).Returns(customer);
        _userManager.FindByIdAsync(recordedById.ToString()).Returns(user);

        var command = new PayRegistrationFeeCommand(
            customerId,
            500m,
            "TX12345",
            "REF98765",
            PaymentMethod.MobilePayment,
            recordedById);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Customer.RegistrationFeeAlreadyPaid", result.Error.Code);
    }
}
