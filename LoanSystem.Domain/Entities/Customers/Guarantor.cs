using LoanSystem.Domain.Primitives;

namespace LoanSystem.Domain.Entities.Customers;

public sealed class Guarantor : BaseEntity
{
    public Guid CustomerId { get; private set; }
    public Customer Customer { get; private set; } = null!;
    public string Name { get; private set; }
    public string IdNumber { get; private set; }
    public string Phone { get; private set; }
    public decimal AmountGuaranteed { get; private set; }
    public string Relationship { get; private set; }

    public Guarantor(Guid customerId, string name, string idNumber, string phone, decimal amountGuaranteed, string relationship)
    {
        CustomerId = customerId;
        Name = name;
        IdNumber = idNumber;
        Phone = phone;
        AmountGuaranteed = amountGuaranteed;
        Relationship = relationship;
    }

    private Guarantor() { } // EF Core
}
