using LoanSystem.Domain.Enums;
using LoanSystem.Domain.Primitives;

namespace LoanSystem.Domain.Entities.Loans;

public sealed class PaySchedule : BaseEntity
{
    public Guid LoanId { get; private set; }
    public Loan Loan { get; private set; } = null!;
    public DateOnly ScheduledDate { get; private set; }
    public decimal Amount { get; private set; }
    public decimal Balance { get; private set; }
    public PaymentStatus Status { get; private set; }

    public PaySchedule(Guid loanId, DateOnly scheduledDate, decimal amount)
    {
        LoanId = loanId;
        ScheduledDate = scheduledDate;
        Amount = amount;
        Balance = amount;
        Status = PaymentStatus.Unpaid;
    }

    private PaySchedule() { } // EF Core
}
