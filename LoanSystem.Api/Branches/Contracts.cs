namespace LoanSystem.Api.Branches;

public sealed record CreateBranchRequest(string Name, string Location);
public sealed record UpdateBranchRequest(string Name, string Location);
