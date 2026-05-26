namespace LoanSystem.Application.Loans;

public sealed record LoanDeductionResponse(
    Guid Id,
    string Name,
    decimal Amount,
    bool IsApplied,
    DateTime? AppliedAt,
    Guid? AppliedById);
