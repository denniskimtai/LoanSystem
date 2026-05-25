namespace LoanSystem.Api.Customers;

public sealed record AddRefereeRequest(
    string Name,
    string Phone,
    string PhysicalAddress,
    string Relationship);
