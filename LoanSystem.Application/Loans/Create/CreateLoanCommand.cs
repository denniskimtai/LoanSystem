using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Domain.Enums;

namespace LoanSystem.Application.Loans.Create;

public sealed record CreateLoanCommand(
    Guid CustomerId,
    Guid ProductId,
    Guid LoId,
    Guid CoId,
    Guid CreatedById,
    decimal Principal,
    decimal InterestAmount,
    LoanType Type,
    IReadOnlyCollection<CreateLoanAddonInput>? Addons = null,
    IReadOnlyCollection<CreateLoanDeductionInput>? Deductions = null) : ICommand<Guid>;

public sealed record CreateLoanAddonInput(string Name, decimal Amount);
public sealed record CreateLoanDeductionInput(string Name, decimal Amount);
