using LoanSystem.Domain.Entities.Customers;
using LoanSystem.Domain.Enums;
using LoanSystem.Domain.Primitives;

namespace LoanSystem.Domain.Entities.Loans;

public sealed class LoanCollateral : BaseEntity
{
    public Guid? LoanId { get; private set; }
    public Loan? Loan { get; private set; }
    public Guid CustomerId { get; private set; }
    public Customer Customer { get; private set; } = null!;
    public string Category { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public decimal CurrentWorth { get; private set; }
    public string RefNumber { get; private set; }
    public string FileNumber { get; private set; }
    public CollateralStatus Status { get; private set; }
    public DateTime AddedAt { get; private set; }

    public LoanCollateral(Guid customerId, string category, string title, string description, decimal currentWorth, string refNumber, string fileNumber)
    {
        CustomerId = customerId;
        Category = category;
        Title = title;
        Description = description;
        CurrentWorth = currentWorth;
        RefNumber = refNumber;
        FileNumber = fileNumber;
        Status = CollateralStatus.NotUsed;
        AddedAt = DateTime.UtcNow;
    }

    private LoanCollateral() { } // EF Core
}
