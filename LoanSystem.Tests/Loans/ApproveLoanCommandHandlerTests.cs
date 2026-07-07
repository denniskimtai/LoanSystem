using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Application.Loans.Approve;
using LoanSystem.Domain.Entities.Identity;
using LoanSystem.Domain.Entities.Loans;
using LoanSystem.Domain.Enums;
using LoanSystem.Domain.Primitives;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using Xunit;

namespace LoanSystem.Tests.Loans;

public class ApproveLoanCommandHandlerTests
{
    private readonly ILoanRepository _loanRepository;
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApproveLoanCommandHandler _handler;

    public ApproveLoanCommandHandlerTests()
    {
        _loanRepository = Substitute.For<ILoanRepository>();
        var store = Substitute.For<IUserStore<User>>();
        _userManager = Substitute.For<UserManager<User>>(store, null, null, null, null, null, null, null, null);
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new ApproveLoanCommandHandler(_loanRepository, _userManager, _unitOfWork);
    }

    [Fact]
    public async Task Handle_CollectionOfficer_Should_Transition_To_BranchApproval_When_Initiation()
    {
        // Arrange
        var loanId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var loan = new Loan("LN-000001", Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 10000m, 2500m, LoanType.Manual);
        // Stage starts at Initiation, Status starts at Created

        var user = new User("co@test.com", "Collection Officer", UserRole.CollectionOfficer, Guid.NewGuid());

        _loanRepository.GetByIdAsync(loanId, Arg.Any<CancellationToken>()).Returns(loan);
        _userManager.FindByIdAsync(userId.ToString()).Returns(user);

        var command = new ApproveLoanCommand(loanId, userId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(LoanStage.BranchApproval, loan.Stage);
        Assert.Equal(LoanStatus.Created, loan.Status);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_Manager_Should_Transition_To_FinalApproval_When_BranchApproval()
    {
        // Arrange
        var loanId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var loan = new Loan("LN-000001", Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 10000m, 2500m, LoanType.Manual);
        loan.UpdateStage(LoanStage.BranchApproval);

        var user = new User("mgr@test.com", "Branch Manager", UserRole.Manager, Guid.NewGuid());

        _loanRepository.GetByIdAsync(loanId, Arg.Any<CancellationToken>()).Returns(loan);
        _userManager.FindByIdAsync(userId.ToString()).Returns(user);

        var command = new ApproveLoanCommand(loanId, userId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(LoanStage.FinalApproval, loan.Stage);
        Assert.Equal(LoanStatus.Created, loan.Status);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_Admin_Should_Approve_Fully_From_Any_Stage()
    {
        // Arrange
        var loanId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var loan = new Loan("LN-000001", Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 10000m, 2500m, LoanType.Manual);
        // Stage is Initiation

        var user = new User("admin@test.com", "Admin User", UserRole.Admin, Guid.NewGuid());

        _loanRepository.GetByIdAsync(loanId, Arg.Any<CancellationToken>()).Returns(loan);
        _userManager.FindByIdAsync(userId.ToString()).Returns(user);

        var command = new ApproveLoanCommand(loanId, userId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(LoanStage.FinalApproval, loan.Stage);
        Assert.Equal(LoanStatus.Approved, loan.Status);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_CollectionOfficer_Should_Fail_When_Not_Initiation()
    {
        // Arrange
        var loanId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var loan = new Loan("LN-000001", Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 10000m, 2500m, LoanType.Manual);
        loan.UpdateStage(LoanStage.BranchApproval); // Stage is already BranchApproval

        var user = new User("co@test.com", "Collection Officer", UserRole.CollectionOfficer, Guid.NewGuid());

        _loanRepository.GetByIdAsync(loanId, Arg.Any<CancellationToken>()).Returns(loan);
        _userManager.FindByIdAsync(userId.ToString()).Returns(user);

        var command = new ApproveLoanCommand(loanId, userId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Loan.InvalidStageTransition", result.Error.Code);
    }

    [Fact]
    public async Task Handle_LoanOfficer_Should_Fail()
    {
        // Arrange
        var loanId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var loan = new Loan("LN-000001", Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 10000m, 2500m, LoanType.Manual);

        var user = new User("lo@test.com", "Loan Officer", UserRole.LoanOfficer, Guid.NewGuid());

        _loanRepository.GetByIdAsync(loanId, Arg.Any<CancellationToken>()).Returns(loan);
        _userManager.FindByIdAsync(userId.ToString()).Returns(user);

        var command = new ApproveLoanCommand(loanId, userId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Loan.UnauthorizedApproval", result.Error.Code);
    }
}
