using LoanSystem.Application.Customers.Create;

namespace LoanSystem.Api.Customers;

public sealed record CreateCustomerRequest(
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
    CreateBusinessInfoInput? BusinessInfo = null,
    CreateSecondaryInfoInput? SecondaryInfo = null,
    IReadOnlyCollection<CreateGuarantorInput>? Guarantors = null,
    IReadOnlyCollection<CreateRefereeInput>? Referees = null);
