using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Application.Loans.Delete;
using LoanSystem.Domain.Entities.Loans;
using LoanSystem.Domain.Enums;
using LoanSystem.Domain.Primitives;
using NSubstitute;
using Xunit;

namespace LoanSystem.Tests.Loans;

public class DeleteLoanCommandHandlerTests
{
    private readonly ILoanRepository _loanRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly DeleteLoanCommandHandler _handler;

    public DeleteLoanCommandHandlerTests()
    {
        _loanRepository = Substitute.For<ILoanRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new DeleteLoanCommandHandler(_loanRepository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenLoanIsDeletedSuccessfully()
    {
        // Arrange
        var loanId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var loId = Guid.NewGuid();
        var coId = Guid.NewGuid();
        var creatorId = Guid.NewGuid();

        // Default constructor sets status = Created, stage = Initiation
        var loan = new Loan("LN-000001", customerId, productId, branchId, loId, coId, creatorId, 15000m, 1800m, LoanType.Manual);

        _loanRepository.GetByIdAsync(loanId, Arg.Any<CancellationToken>()).Returns(loan);

        var command = new DeleteLoanCommand(loanId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(loan.IsDeleted);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenLoanDoesNotExist()
    {
        // Arrange
        var loanId = Guid.NewGuid();
        _loanRepository.GetByIdAsync(loanId, Arg.Any<CancellationToken>()).Returns((Loan?)null);

        var command = new DeleteLoanCommand(loanId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Loan.NotFound", result.Error.Code);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenLoanIsApproved()
    {
        // Arrange
        var loanId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var loId = Guid.NewGuid();
        var coId = Guid.NewGuid();
        var creatorId = Guid.NewGuid();

        var loan = new Loan("LN-000001", customerId, productId, branchId, loId, coId, creatorId, 15000m, 1800m, LoanType.Manual);
        loan.UpdateStatus(LoanStatus.Approved);

        _loanRepository.GetByIdAsync(loanId, Arg.Any<CancellationToken>()).Returns(loan);

        var command = new DeleteLoanCommand(loanId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Loan.DeleteBlocked", result.Error.Code);
        Assert.False(loan.IsDeleted);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenLoanStageIsNoLongerInitiation()
    {
        // Arrange
        var loanId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var loId = Guid.NewGuid();
        var coId = Guid.NewGuid();
        var creatorId = Guid.NewGuid();

        var loan = new Loan("LN-000001", customerId, productId, branchId, loId, coId, creatorId, 15000m, 1800m, LoanType.Manual);
        loan.UpdateStage(LoanStage.BranchApproval);

        _loanRepository.GetByIdAsync(loanId, Arg.Any<CancellationToken>()).Returns(loan);

        var command = new DeleteLoanCommand(loanId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Loan.DeleteBlocked", result.Error.Code);
        Assert.False(loan.IsDeleted);
    }
}
