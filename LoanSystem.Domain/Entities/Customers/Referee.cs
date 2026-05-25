using LoanSystem.Domain.Primitives;

namespace LoanSystem.Domain.Entities.Customers;

public sealed class Referee : BaseEntity
{
    public Guid CustomerId { get; private set; }
    public Customer Customer { get; private set; } = null!;
    public string Name { get; private set; }
    public string Phone { get; private set; }
    public string PhysicalAddress { get; private set; }
    public string Relationship { get; private set; }

    public Referee(Guid customerId, string name, string phone, string physicalAddress, string relationship)
    {
        CustomerId = customerId;
        Name = name;
        Phone = phone;
        PhysicalAddress = physicalAddress;
        Relationship = relationship;
    }

    private Referee() { } // EF Core

    public void Update(
        string name,
        string phone,
        string physicalAddress,
        string relationship)
    {
        Name = name;
        Phone = phone;
        PhysicalAddress = physicalAddress;
        Relationship = relationship;
        UpdateTimestamp();
    }
}
