using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Domain.Enums;
using LoanSystem.Domain.Primitives;
using LoanSystem.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace LoanSystem.Application.Loans.Reject;

public sealed class RejectLoanCommandHandler : ICommandHandler<RejectLoanCommand>
{
    private readonly ILoanRepository _loanRepository;
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public RejectLoanCommandHandler(
        ILoanRepository loanRepository,
        UserManager<User> userManager,
        IUnitOfWork unitOfWork)
    {
        _loanRepository = loanRepository;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(RejectLoanCommand request, CancellationToken cancellationToken)
    {
        // 1. Get Loan
        var loan = await _loanRepository.GetByIdAsync(request.Id, cancellationToken);
        if (loan is null)
        {
            return Result.Failure(new Error("Loan.NotFound", "The specified loan does not exist."));
        }

        // 2. Validate current state (only allow rejection if Created)
        if (loan.Status != LoanStatus.Created)
        {
            return Result.Failure(new Error("Loan.InvalidState", $"Loans in the {loan.Status} status cannot be rejected."));
        }

        // 3. Get User and verify role
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
        {
            return Result.Failure(new Error("User.NotFound", "The specified user does not exist."));
        }

        if (user.Role != UserRole.CollectionOfficer && user.Role != UserRole.Manager && user.Role != UserRole.Admin)
        {
            return Result.Failure(new Error("Loan.UnauthorizedRejection", "The user's role is not authorized to reject loans."));
        }

        // 4. Update status
        loan.UpdateStatus(LoanStatus.Rejected);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
