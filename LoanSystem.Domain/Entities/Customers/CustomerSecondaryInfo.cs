using LoanSystem.Domain.Enums;
using LoanSystem.Domain.Primitives;

namespace LoanSystem.Domain.Entities.Customers;

public sealed class CustomerSecondaryInfo : BaseEntity
{
    public Guid CustomerId { get; private set; }
    public Customer Customer { get; private set; } = null!;
    public MaritalStatus MaritalStatus { get; private set; }
    public int Dependents { get; private set; }
    public string Estate { get; private set; }
    public string HouseNumber { get; private set; }
    public HomeOwnership Ownership { get; private set; }
    public decimal? RentAmount { get; private set; }
    public decimal HomeAssetValue { get; private set; }
    public string NearestLandmark { get; private set; }
    public string GeoLocation { get; private set; }
    public string HeardVia { get; private set; }

    public CustomerSecondaryInfo(Guid customerId, MaritalStatus maritalStatus, int dependents, string estate, string houseNumber, HomeOwnership ownership, decimal? rentAmount, decimal homeAssetValue, string nearestLandmark, string geoLocation, string heardVia)
    {
        CustomerId = customerId;
        MaritalStatus = maritalStatus;
        Dependents = dependents;
        Estate = estate;
        HouseNumber = houseNumber;
        Ownership = ownership;
        RentAmount = rentAmount;
        HomeAssetValue = homeAssetValue;
        NearestLandmark = nearestLandmark;
        GeoLocation = geoLocation;
        HeardVia = heardVia;
    }

    private CustomerSecondaryInfo() { } // EF Core
}
