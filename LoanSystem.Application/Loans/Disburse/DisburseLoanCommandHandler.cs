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

        // 3. Verify Registration Fee for new clients
        var isRepeat = await _loanRepository.HasAnyOtherLoansAsync(loan.CustomerId, loan.Id, cancellationToken);
        if (!isRepeat && !loan.Customer.RegistrationFeePaid)
        {
            return Result.Failure(new Error("Customer.RegistrationFeeNotPaid", "The customer must pay the member registration fee of 500 Ksh before loan disbursement."));
        }

        // 4. Calculate Due Date using Product's RepaymentDays
        var dueDate = DateOnly.FromDateTime(request.DisbursedAt.AddDays(loan.Product.RepaymentDays));

        // 5. Update status, stage, disbursement details
        loan.SetDisbursed(request.DisbursedAt, dueDate, request.MpesaCode);

        // 6. Generate and add daily PaySchedules for the total repayable amount
        loan.ClearPaySchedules();
        var repaymentDays = loan.Product.RepaymentDays;
        var dailyAmount = Math.Round(loan.RepayableTotal / repaymentDays, 2);
        var accumulated = 0m;

        for (int i = 1; i < repaymentDays; i++)
        {
            var scheduleDate = DateOnly.FromDateTime(request.DisbursedAt.AddDays(i));
            var paySchedule = new PaySchedule(loan.Id, scheduleDate, dailyAmount);
            loan.AddPaySchedule(paySchedule);
            accumulated += dailyAmount;
        }

        // Adjust the last installment for precision
        var finalScheduleDate = DateOnly.FromDateTime(request.DisbursedAt.AddDays(repaymentDays));
        var finalAmount = loan.RepayableTotal - accumulated;
        var finalPaySchedule = new PaySchedule(loan.Id, finalScheduleDate, finalAmount);
        loan.AddPaySchedule(finalPaySchedule);

        // 7. Save changes
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
