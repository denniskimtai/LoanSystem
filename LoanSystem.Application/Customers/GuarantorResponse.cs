namespace LoanSystem.Application.Customers;

public sealed record GuarantorResponse(
    Guid Id,
    string Name,
    string IdNumber,
    string Phone,
    decimal AmountGuaranteed,
    string Relationship,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
