using LoanSystem.Domain.Enums;

namespace LoanSystem.Api.Customers;

public sealed record UpdateCustomerSecondaryInfoRequest(
    MaritalStatus MaritalStatus,
    int Dependents,
    string Estate,
    string HouseNumber,
    HomeOwnership Ownership,
    decimal? RentAmount,
    decimal HomeAssetValue,
    string NearestLandmark,
    string GeoLocation,
    string HeardVia);
