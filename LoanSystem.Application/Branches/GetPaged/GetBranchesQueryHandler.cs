using LoanSystem.Application.Abstractions;
using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Domain.Primitives;

namespace LoanSystem.Application.Branches.GetPaged;

public sealed class GetBranchesQueryHandler : IQueryHandler<GetBranchesQuery, PagedResult<BranchResponse>>
{
    private readonly IBranchRepository _branchRepository;

    public GetBranchesQueryHandler(IBranchRepository branchRepository)
    {
        _branchRepository = branchRepository;
    }

    public async Task<Result<PagedResult<BranchResponse>>> Handle(GetBranchesQuery request, CancellationToken cancellationToken)
    {
        var page = request.Page <= 0 ? 1 : request.Page;
        var pageSize = request.PageSize <= 0 ? 10 : request.PageSize;

        var (items, totalCount) = await _branchRepository.GetPagedAsync(page, pageSize, cancellationToken);

        var responses = items.Select(branch => new BranchResponse(
            branch.Id,
            branch.Name,
            branch.Location,
            branch.CreatedAt,
            branch.UpdatedAt))
            .ToList();

        var result = new PagedResult<BranchResponse>(responses, page, pageSize, totalCount);

        return Result.Success(result);
    }
}
