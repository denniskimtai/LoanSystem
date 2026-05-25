namespace LoanSystem.Application.Customers;

public sealed record RefereeResponse(
    Guid Id,
    string Name,
    string Phone,
    string PhysicalAddress,
    string Relationship,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
