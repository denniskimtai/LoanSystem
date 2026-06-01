using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Application.CRM.Interactions.Create;
using LoanSystem.Domain.Entities.Customers;
using LoanSystem.Domain.Entities.CRM;
using LoanSystem.Domain.Enums;
using LoanSystem.Domain.Primitives;
using NSubstitute;

namespace LoanSystem.Tests.CRM;

public class CreateInteractionCommandHandlerTests
{
    private readonly IInteractionRepository _interactionRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly ILoanRepository _loanRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CreateInteractionCommandHandler _handler;

    public CreateInteractionCommandHandlerTests()
    {
        _interactionRepository = Substitute.For<IInteractionRepository>();
        _customerRepository = Substitute.For<ICustomerRepository>();
        _loanRepository = Substitute.For<ILoanRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new CreateInteractionCommandHandler(
            _interactionRepository,
            _customerRepository,
            _loanRepository,
            _unitOfWork);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenCreateIsSuccessful()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var agentId = Guid.NewGuid();
        var customer = new Customer("Test Customer", "12345678", "0712345678", "http://photo", "Address", "Loc", "Town", "County", "Post", Guid.NewGuid(), agentId);

        _customerRepository.GetByIdAsync(customerId, Arg.Any<CancellationToken>())
            .Returns(customer);

        var command = new CreateInteractionCommand(
            customerId,
            agentId,
            "Call",
            "Follow up",
            "Details",
            "Contactable",
            "Active",
            "None",
            "Call again",
            "1.2,34.5",
            DateTime.UtcNow);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value);
        _interactionRepository.Received(1).Add(Arg.Any<Interaction>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenCustomerDoesNotExist()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var agentId = Guid.NewGuid();

        _customerRepository.GetByIdAsync(customerId, Arg.Any<CancellationToken>())
            .Returns((Customer?)null);

        var command = new CreateInteractionCommand(
            customerId,
            agentId,
            "Call",
            "Follow up",
            "Details",
            "Contactable",
            "Active",
            "None",
            "Call again",
            "1.2,34.5",
            DateTime.UtcNow);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Customer.NotFound", result.Error.Code);
        _interactionRepository.DidNotReceive().Add(Arg.Any<Interaction>());
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenLoanDoesNotExist()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var agentId = Guid.NewGuid();
        var loanId = Guid.NewGuid();
        var customer = new Customer("Test Customer", "12345678", "0712345678", "http://photo", "Address", "Loc", "Town", "County", "Post", Guid.NewGuid(), agentId);

        _customerRepository.GetByIdAsync(customerId, Arg.Any<CancellationToken>())
            .Returns(customer);

        _loanRepository.ExistsAsync(loanId, Arg.Any<CancellationToken>())
            .Returns(false);

        var command = new CreateInteractionCommand(
            customerId,
            agentId,
            "Call",
            "Follow up",
            "Details",
            "Contactable",
            "Active",
            "None",
            "Call again",
            "1.2,34.5",
            DateTime.UtcNow,
            LoanId: loanId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Loan.NotFound", result.Error.Code);
        _interactionRepository.DidNotReceive().Add(Arg.Any<Interaction>());
    }
}
