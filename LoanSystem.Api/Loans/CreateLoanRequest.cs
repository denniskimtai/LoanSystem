using LoanSystem.Application.Loans.Create;
using LoanSystem.Domain.Enums;

namespace LoanSystem.Api.Loans;

public sealed record CreateLoanRequest(
    Guid CustomerId,
    Guid ProductId,
    Guid LoId,
    Guid CoId,
    decimal Principal,
    decimal InterestAmount,
    LoanType Type,
    IReadOnlyCollection<CreateLoanAddonInput>? Addons = null,
    IReadOnlyCollection<CreateLoanDeductionInput>? Deductions = null);
