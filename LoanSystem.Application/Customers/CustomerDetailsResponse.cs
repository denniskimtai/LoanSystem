using LoanSystem.Domain.Enums;

namespace LoanSystem.Application.Customers;

public sealed record CustomerDetailsResponse(
    Guid Id,
    string FullName,
    string NationalId,
    string Phone,
    string PhotoUrl,
    CustomerStatus Status,
    string PhysicalAddress,
    string HomeGeoLocation,
    string Town,
    string County,
    string PostalAddress,
    decimal CurrentLimit,
    Guid? CurrentLoId,
    Guid? CurrentCoId,
    Guid BranchId,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    CustomerBusinessInfoResponse? BusinessInfo,
    CustomerSecondaryInfoResponse? SecondaryInfo,
    IReadOnlyCollection<GuarantorResponse> Guarantors,
    IReadOnlyCollection<RefereeResponse> Referees);
