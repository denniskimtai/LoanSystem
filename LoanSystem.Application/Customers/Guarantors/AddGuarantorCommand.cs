using LoanSystem.Application.Abstractions.Messaging;

namespace LoanSystem.Application.Customers.Guarantors;

public sealed record AddGuarantorCommand(
    Guid CustomerId,
    string Name,
    string IdNumber,
    string Phone,
    decimal AmountGuaranteed,
    string Relationship) : ICommand<Guid>;
