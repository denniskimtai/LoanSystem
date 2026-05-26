namespace LoanSystem.Application.Loans;

public sealed record LoanProductResponse(
    Guid Id,
    string Name,
    decimal MinAmount,
    decimal MaxAmount,
    decimal InterestRate,
    int RepaymentDays);
