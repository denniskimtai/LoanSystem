using LoanSystem.Domain.Entities.CRM;
using LoanSystem.Domain.Entities.Customers;
using LoanSystem.Domain.Entities.Identity;
using LoanSystem.Domain.Entities.Payments;
using LoanSystem.Domain.Enums;
using LoanSystem.Domain.Primitives;

namespace LoanSystem.Domain.Entities.Loans;

public sealed class Loan : BaseEntity
{
    public string Code { get; private set; }
    public Guid CustomerId { get; private set; }
    public Customer Customer { get; private set; } = null!;
    public Guid ProductId { get; private set; }
    public LoanProduct Product { get; private set; } = null!;
    public Guid BranchId { get; private set; }
    public Branch Branch { get; private set; } = null!;
    public Guid LoId { get; private set; }
    public User Lo { get; private set; } = null!;
    public Guid CoId { get; private set; }
    public User Co { get; private set; } = null!;
    public Guid CreatedById { get; private set; }
    public User CreatedBy { get; private set; } = null!;
    
    public decimal Principal { get; private set; }
    public decimal AddOnsTotal { get; private set; }
    public decimal DeductionsTotal { get; private set; }
    public decimal RepayableTotal { get; private set; }
    public decimal RepaidTotal { get; private set; }
    public decimal Balance { get; private set; }
    public decimal InterestAmount { get; private set; }
    public decimal PenaltyAmount { get; private set; }
    
    public LoanType Type { get; private set; }
    public LoanStage Stage { get; private set; }
    public LoanStatus Status { get; private set; }
    public string? MpesaCode { get; private set; }
    public DateTime? DisbursedAt { get; private set; }
    public DateOnly? DueDate { get; private set; }
    public DateOnly? LastRepayDate { get; private set; }
    public DateOnly? ClearedDate { get; private set; }

    public IReadOnlyCollection<LoanAddon> Addons => _addons.AsReadOnly();
    private readonly List<LoanAddon> _addons = new();

    public IReadOnlyCollection<LoanDeduction> Deductions => _deductions.AsReadOnly();
    private readonly List<LoanDeduction> _deductions = new();

    public IReadOnlyCollection<PaySchedule> PaySchedules => _paySchedules.AsReadOnly();
    private readonly List<PaySchedule> _paySchedules = new();

    public ICollection<LoanCollateral> Collaterals { get; private set; } = new List<LoanCollateral>();
    public ICollection<Payment> Payments { get; private set; } = new List<Payment>();
    public ICollection<Interaction> Interactions { get; private set; } = new List<Interaction>();

    public Loan(string code, Guid customerId, Guid productId, Guid branchId, Guid loId, Guid coId, Guid createdById, decimal principal, decimal interestAmount, LoanType type)
    {
        Code = code;
        CustomerId = customerId;
        ProductId = productId;
        BranchId = branchId;
        LoId = loId;
        CoId = coId;
        CreatedById = createdById;
        Principal = principal;
        InterestAmount = interestAmount;
        Type = type;
        Stage = LoanStage.Initiation;
        Status = LoanStatus.Created;
        
        // Initial calculations
        RepayableTotal = principal + interestAmount;
        Balance = RepayableTotal;
    }

    private Loan() { } // EF Core
}
