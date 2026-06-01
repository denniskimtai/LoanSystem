using LoanSystem.Application.Abstractions.Messaging;

namespace LoanSystem.Application.LoanProducts.Update;

public sealed record UpdateLoanProductCommand(
    Guid Id,
    string Name,
    decimal MinAmount,
    decimal MaxAmount,
    decimal InterestRate,
    int RepaymentDays) : ICommand;
