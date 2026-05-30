using LoanSystem.Domain.Enums;

namespace LoanSystem.Api.Loans;

public sealed record UpdateLoanRequest(
    decimal Principal,
    decimal InterestAmount,
    Guid ProductId,
    Guid LoId,
    Guid CoId,
    LoanType Type);
