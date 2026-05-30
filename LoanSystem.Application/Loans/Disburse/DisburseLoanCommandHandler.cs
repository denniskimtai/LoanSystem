using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Domain.Entities.Loans;
using LoanSystem.Domain.Enums;
using LoanSystem.Domain.Primitives;

namespace LoanSystem.Application.Loans.Disburse;

public sealed class DisburseLoanCommandHandler : ICommandHandler<DisburseLoanCommand>
{
    private readonly ILoanRepository _loanRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DisburseLoanCommandHandler(ILoanRepository loanRepository, IUnitOfWork unitOfWork)
    {
        _loanRepository = loanRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DisburseLoanCommand request, CancellationToken cancellationToken)
    {
        // 1. Retrieve Loan with Details
        var loan = await _loanRepository.GetByIdWithDetailsAsync(request.Id, cancellationToken);
        if (loan is null)
        {
            return Result.Failure(new Error("Loan.NotFound", "The specified loan does not exist."));
        }

        // 2. Validate current state (only allow disbursement if Approved)
        if (loan.Status != LoanStatus.Approved)
        {
            return Result.Failure(new Error("Loan.InvalidState", $"Loans in the {loan.Status} status cannot be disbursed."));
        }

        // 3. Calculate Due Date using Product's RepaymentDays
        var dueDate = DateOnly.FromDateTime(request.DisbursedAt.AddDays(loan.Product.RepaymentDays));

        // 4. Update status, stage, disbursement details
        loan.SetDisbursed(request.DisbursedAt, dueDate, request.MpesaCode);

        // 5. Generate and add a single PaySchedule for the total repayable amount
        loan.ClearPaySchedules();
        var paySchedule = new PaySchedule(loan.Id, dueDate, loan.RepayableTotal);
        loan.AddPaySchedule(paySchedule);

        // 6. Save changes
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
