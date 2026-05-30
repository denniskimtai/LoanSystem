namespace LoanSystem.Application.Loans;

public sealed record LoanAddonResponse(
    Guid Id,
    string Name,
    decimal Amount,
    bool IsApplied,
    DateTime? AppliedAt,
    Guid? AppliedById);
