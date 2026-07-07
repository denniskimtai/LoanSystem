namespace LoanSystem.Api.LoanProducts;

public sealed record UpdateLoanProductRequest(
    string Name,
    decimal MinAmount,
    decimal MaxAmount,
    decimal InterestRate,
    int RepaymentDays);
