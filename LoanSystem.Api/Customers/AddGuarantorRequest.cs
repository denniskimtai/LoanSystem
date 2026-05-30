namespace LoanSystem.Api.Customers;

public sealed record AddGuarantorRequest(
    string Name,
    string IdNumber,
    string Phone,
    decimal AmountGuaranteed,
    string Relationship);
