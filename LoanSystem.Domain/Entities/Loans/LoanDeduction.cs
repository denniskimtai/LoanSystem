using LoanSystem.Domain.Entities.Identity;
using LoanSystem.Domain.Primitives;

namespace LoanSystem.Domain.Entities.Loans;

public sealed class LoanDeduction : BaseEntity
{
    public Guid LoanId { get; private set; }
    public Loan Loan { get; private set; } = null!;
    public string Name { get; private set; }
    public decimal Amount { get; private set; }
    public bool IsApplied { get; private set; }
    public DateTime? AppliedAt { get; private set; }
    public Guid? AppliedById { get; private set; }
    public User? AppliedBy { get; private set; }

    public LoanDeduction(Guid loanId, string name, decimal amount)
    {
        LoanId = loanId;
        Name = name;
        Amount = amount;
        IsApplied = false;
    }

    private LoanDeduction() { } // EF Core
}
