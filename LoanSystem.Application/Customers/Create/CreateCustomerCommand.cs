using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Domain.Enums;

namespace LoanSystem.Application.Customers.Create;

public sealed record CreateCustomerCommand(
    string FullName,
    string NationalId,
    string Phone,
    string? PhotoUrl,
    string PhysicalAddress,
    string? HomeGeoLocation,
    string Town,
    string County,
    string PostalAddress,
    Guid BranchId,
    Guid CreatedById,
    CreateBusinessInfoInput? BusinessInfo = null,
    CreateSecondaryInfoInput? SecondaryInfo = null,
    IReadOnlyCollection<CreateGuarantorInput>? Guarantors = null,
    IReadOnlyCollection<CreateRefereeInput>? Referees = null) : ICommand<Guid>;

public sealed record CreateBusinessInfoInput(
    string BusinessName,
    string BusinessType,
    string BusinessDirection,
    string BusinessGeoLocation,
    decimal CurrentStockValue,
    decimal WeeklyGrossProfit,
    decimal WeeklyNetProfit,
    decimal WeeklyExpenses,
    int YearsInBusiness,
    bool OffersCredit,
    string LeadType,
    decimal ProposedLimit,
    bool WouldLend);

public sealed record CreateSecondaryInfoInput(
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

public sealed record CreateGuarantorInput(
    string Name,
    string IdNumber,
    string Phone,
    decimal AmountGuaranteed,
    string Relationship);

public sealed record CreateRefereeInput(
    string Name,
    string Phone,
    string PhysicalAddress,
    string Relationship);
