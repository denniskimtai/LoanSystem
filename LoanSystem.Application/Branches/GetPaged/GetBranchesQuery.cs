using LoanSystem.Application.Abstractions;
using LoanSystem.Application.Abstractions.Messaging;

namespace LoanSystem.Application.Branches.GetPaged;

public sealed record GetBranchesQuery(
    int Page,
    int PageSize) : IQuery<PagedResult<BranchResponse>>;
