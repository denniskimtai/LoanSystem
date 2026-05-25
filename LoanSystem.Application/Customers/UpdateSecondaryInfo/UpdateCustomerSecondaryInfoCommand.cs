using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Domain.Enums;

namespace LoanSystem.Application.Customers.UpdateSecondaryInfo;

public sealed record UpdateCustomerSecondaryInfoCommand(
    Guid CustomerId,
    MaritalStatus MaritalStatus,
    int Dependents,
    string Estate,
    string HouseNumber,
    HomeOwnership Ownership,
    decimal? RentAmount,
    decimal HomeAssetValue,
    string NearestLandmark,
    string GeoLocation,
    string HeardVia) : ICommand;
