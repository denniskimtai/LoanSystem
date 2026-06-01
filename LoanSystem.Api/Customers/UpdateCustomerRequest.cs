namespace LoanSystem.Api.Customers;

public sealed record UpdateCustomerRequest(
    string FullName,
    string NationalId,
    string Phone,
    string? PhotoUrl,
    string PhysicalAddress,
    string? HomeGeoLocation,
    string Town,
    string County,
    string PostalAddress);
