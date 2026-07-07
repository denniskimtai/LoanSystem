using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Domain.Enums;
using LoanSystem.Domain.Primitives;
using LoanSystem.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace LoanSystem.Application.Loans.Approve;

public sealed class ApproveLoanCommandHandler : ICommandHandler<ApproveLoanCommand>
{
    private readonly ILoanRepository _loanRepository;
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public ApproveLoanCommandHandler(
        ILoanRepository loanRepository,
        UserManager<User> userManager,
        IUnitOfWork unitOfWork)
    {
        _loanRepository = loanRepository;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ApproveLoanCommand request, CancellationToken cancellationToken)
    {
        // 1. Get Loan
        var loan = await _loanRepository.GetByIdAsync(request.Id, cancellationToken);
        if (loan is null)
        {
            return Result.Failure(new Error("Loan.NotFound", "The specified loan does not exist."));
        }

        // 2. Validate current state (only allow approval if Created)
        if (loan.Status != LoanStatus.Created)
        {
            return Result.Failure(new Error("Loan.InvalidState", $"Loans in the {loan.Status} status cannot be approved."));
        }

        // 3. Get User and verify role
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
        {
            return Result.Failure(new Error("User.NotFound", "The specified user does not exist."));
        }

        // 4. Update status and stage based on user's role
        if (user.Role == UserRole.CollectionOfficer)
        {
            if (loan.Stage != LoanStage.Initiation)
            {
                return Result.Failure(new Error("Loan.InvalidStageTransition", "Collection Officer can only approve loans in the Initiation stage."));
            }
            loan.UpdateStage(LoanStage.BranchApproval);
        }
        else if (user.Role == UserRole.Manager)
        {
            if (loan.Stage != LoanStage.BranchApproval)
            {
                return Result.Failure(new Error("Loan.InvalidStageTransition", "Branch Manager can only approve loans in the BranchApproval stage."));
            }
            loan.UpdateStage(LoanStage.FinalApproval);
        }
        else if (user.Role == UserRole.Admin)
        {
            // Admin can approve from any stage, directly setting to FinalApproval and Approved status
            loan.UpdateStage(LoanStage.FinalApproval);
            loan.UpdateStatus(LoanStatus.Approved);
        }
        else
        {
            return Result.Failure(new Error("Loan.UnauthorizedApproval", "The user's role is not authorized to approve loans."));
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
