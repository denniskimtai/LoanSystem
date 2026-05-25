namespace LoanSystem.Application.Branches;

public sealed record BranchResponse(
    Guid Id,
    string Name,
    string Location,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
