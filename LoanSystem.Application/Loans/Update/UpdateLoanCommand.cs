using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Domain.Enums;

namespace LoanSystem.Application.Loans.Update;

public sealed record UpdateLoanCommand(
    Guid Id,
    decimal Principal,
    decimal InterestAmount,
    Guid ProductId,
    Guid LoId,
    Guid CoId,
    LoanType Type) : ICommand;
