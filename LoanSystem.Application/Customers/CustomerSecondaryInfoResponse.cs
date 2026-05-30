using LoanSystem.Domain.Enums;

namespace LoanSystem.Application.Customers;

public sealed record CustomerSecondaryInfoResponse(
    Guid Id,
    MaritalStatus MaritalStatus,
    int Dependents,
    string Estate,
    string HouseNumber,
    HomeOwnership Ownership,
    decimal? RentAmount,
    decimal HomeAssetValue,
    string NearestLandmark,
    string GeoLocation,
    string HeardVia,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
