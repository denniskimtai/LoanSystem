using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Application.Loans.Disburse;
using LoanSystem.Domain.Entities.Loans;
using LoanSystem.Domain.Enums;
using LoanSystem.Domain.Primitives;
using NSubstitute;
using Xunit;

namespace LoanSystem.Tests.Loans;

public class DisburseLoanCommandHandlerTests
{
    private readonly ILoanRepository _loanRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly DisburseLoanCommandHandler _handler;

    public DisburseLoanCommandHandlerTests()
    {
        _loanRepository = Substitute.For<ILoanRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new DisburseLoanCommandHandler(_loanRepository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenDisbursementIsSuccessful()
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

        var product = new LoanProduct("Test Product", 1000m, 50000m, 0.12m, 30);
        
        // Set Product navigation property using reflection since it has a private setter
        typeof(Loan).GetProperty("Product")!.SetValue(loan, product);

        _loanRepository.GetByIdWithDetailsAsync(loanId, Arg.Any<CancellationToken>()).Returns(loan);

        var disburseDate = DateTime.UtcNow;
        var command = new DisburseLoanCommand(loanId, "MPESA123XYZ", disburseDate);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(LoanStatus.Disbursed, loan.Status);
        Assert.Equal("MPESA123XYZ", loan.MpesaCode);
        Assert.Equal(disburseDate, loan.DisbursedAt);
        Assert.Equal(DateOnly.FromDateTime(disburseDate.AddDays(product.RepaymentDays)), loan.DueDate);
        Assert.Single(loan.PaySchedules);
        Assert.Equal(loan.RepayableTotal, loan.PaySchedules.First().Amount);
        Assert.Equal(PaymentStatus.Unpaid, loan.PaySchedules.First().Status);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenLoanDoesNotExist()
    {
        // Arrange
        var loanId = Guid.NewGuid();
        _loanRepository.GetByIdWithDetailsAsync(loanId, Arg.Any<CancellationToken>()).Returns((Loan?)null);

        var command = new DisburseLoanCommand(loanId, "MPESA123XYZ", DateTime.UtcNow);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Loan.NotFound", result.Error.Code);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenLoanIsNotApproved()
    {
        // Arrange
        var loanId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var loId = Guid.NewGuid();
        var coId = Guid.NewGuid();
        var creatorId = Guid.NewGuid();

        // Loan remains in Created status
        var loan = new Loan("LN-000001", customerId, productId, branchId, loId, coId, creatorId, 15000m, 1800m, LoanType.Manual);

        _loanRepository.GetByIdWithDetailsAsync(loanId, Arg.Any<CancellationToken>()).Returns(loan);

        var command = new DisburseLoanCommand(loanId, "MPESA123XYZ", DateTime.UtcNow);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Loan.InvalidState", result.Error.Code);
    }
}
