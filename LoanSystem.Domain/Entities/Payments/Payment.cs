using LoanSystem.Domain.Entities.Customers;
using LoanSystem.Domain.Entities.Identity;
using LoanSystem.Domain.Entities.Loans;
using LoanSystem.Domain.Enums;
using LoanSystem.Domain.Primitives;

namespace LoanSystem.Domain.Entities.Payments;

public sealed class Payment : BaseEntity
{
    public Guid? LoanId { get; private set; }
    public Loan? Loan { get; private set; }
    public Guid CustomerId { get; private set; }
    public Customer Customer { get; private set; } = null!;
    public Guid RecordedById { get; private set; }
    public User RecordedBy { get; private set; } = null!;
    public decimal Amount { get; private set; }
    public string TransactionCode { get; private set; }
    public string MpesaRef { get; private set; }
    public PaymentMethod PayMethod { get; private set; }
    public RecordType RecordType { get; private set; }
    public bool IsAllocated { get; private set; }
    public DateTime PaidAt { get; private set; }

    public Payment(Guid? loanId, Guid customerId, Guid recordedById, decimal amount, string transactionCode, string mpesaRef, PaymentMethod payMethod, RecordType recordType, DateTime paidAt)
    {
        LoanId = loanId;
        CustomerId = customerId;
        RecordedById = recordedById;
        Amount = amount;
        TransactionCode = transactionCode;
        MpesaRef = mpesaRef;
        PayMethod = payMethod;
        RecordType = recordType;
        IsAllocated = true;
        PaidAt = paidAt;
    }

    private Payment() { } // EF Core
}
