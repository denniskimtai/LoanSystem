using LoanSystem.Application.Abstractions.Messaging;

namespace LoanSystem.Application.Customers.Update;

public sealed record UpdateCustomerCommand(
    Guid Id,
    string FullName,
    string NationalId,
    string Phone,
    string? PhotoUrl,
    string PhysicalAddress,
    string? HomeGeoLocation,
    string Town,
    string County,
    string PostalAddress) : ICommand;
