using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Domain.Primitives;

namespace LoanSystem.Application.Branches.GetById;

public sealed class GetBranchByIdQueryHandler : IQueryHandler<GetBranchByIdQuery, BranchResponse>
{
    private readonly IBranchRepository _branchRepository;

    public GetBranchByIdQueryHandler(IBranchRepository branchRepository)
    {
        _branchRepository = branchRepository;
    }

    public async Task<Result<BranchResponse>> Handle(GetBranchByIdQuery request, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.GetByIdAsync(request.Id, cancellationToken);

        if (branch is null)
        {
            return Result.Failure<BranchResponse>(new Error("Branch.NotFound", "The specified branch does not exist."));
        }

        var response = new BranchResponse(
            branch.Id,
            branch.Name,
            branch.Location,
            branch.CreatedAt,
            branch.UpdatedAt);

        return Result.Success(response);
    }
}
