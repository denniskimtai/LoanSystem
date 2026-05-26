using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Domain.Enums;
using LoanSystem.Domain.Primitives;

namespace LoanSystem.Application.Loans.Approve;

public sealed class ApproveLoanCommandHandler : ICommandHandler<ApproveLoanCommand>
{
    private readonly ILoanRepository _loanRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ApproveLoanCommandHandler(ILoanRepository loanRepository, IUnitOfWork unitOfWork)
    {
        _loanRepository = loanRepository;
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

        // 3. Update status and stage
        loan.UpdateStatus(LoanStatus.Approved);
        loan.UpdateStage(LoanStage.FinalApproval);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
