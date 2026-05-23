using LoanSystem.Domain.Entities.CRM;
using LoanSystem.Domain.Entities.Identity;
using LoanSystem.Domain.Entities.Loans;
using LoanSystem.Domain.Entities.Payments;
using LoanSystem.Domain.Enums;
using LoanSystem.Domain.Primitives;

namespace LoanSystem.Domain.Entities.Customers;

public sealed class Customer : BaseEntity
{
    public string FullName { get; private set; }
    public string NationalId { get; private set; }
    public string Phone { get; private set; }
    public string PhotoUrl { get; private set; }
    public CustomerStatus Status { get; private set; }
    public string PhysicalAddress { get; private set; }
    public string HomeGeoLocation { get; private set; }
    public string Town { get; private set; }
    public string County { get; private set; }
    public string PostalAddress { get; private set; }
    public decimal CurrentLimit { get; private set; }
    public Guid? CurrentLoId { get; private set; }
    public User? CurrentLo { get; private set; }
    public Guid? CurrentCoId { get; private set; }
    public User? CurrentCo { get; private set; }
    public Guid BranchId { get; private set; }
    public Branch Branch { get; private set; } = null!;
    public Guid CreatedById { get; private set; }
    public User CreatedBy { get; private set; } = null!;

    // Navigation properties
    public CustomerBusinessInfo? BusinessInfo { get; private set; }
    public CustomerSecondaryInfo? SecondaryInfo { get; private set; }
    public ICollection<Guarantor> Guarantors { get; private set; } = new List<Guarantor>();
    public ICollection<Referee> Referees { get; private set; } = new List<Referee>();
    public ICollection<Loan> Loans { get; private set; } = new List<Loan>();
    public ICollection<Payment> Payments { get; private set; } = new List<Payment>();
    public ICollection<Interaction> Interactions { get; private set; } = new List<Interaction>();

    public Customer(string fullName, string nationalId, string phone, string photoUrl, string physicalAddress, string homeGeoLocation, string town, string county, string postalAddress, Guid branchId, Guid createdById)
    {
        FullName = fullName;
        NationalId = nationalId;
        Phone = phone;
        PhotoUrl = photoUrl;
        Status = CustomerStatus.Lead;
        PhysicalAddress = physicalAddress;
        HomeGeoLocation = homeGeoLocation;
        Town = town;
        County = county;
        PostalAddress = postalAddress;
        CurrentLimit = 0m;
        BranchId = branchId;
        CreatedById = createdById;
    }

    private Customer() { } // EF Core
}
