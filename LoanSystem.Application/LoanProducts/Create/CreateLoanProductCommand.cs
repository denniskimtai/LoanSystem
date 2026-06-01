using LoanSystem.Application.Abstractions.Messaging;

namespace LoanSystem.Application.LoanProducts.Create;

public sealed record CreateLoanProductCommand(
    string Name,
    decimal MinAmount,
    decimal MaxAmount,
    decimal InterestRate,
    int RepaymentDays) : ICommand<Guid>;
