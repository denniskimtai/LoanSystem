using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Domain.Primitives;

namespace LoanSystem.Application.Branches.Update;

public sealed class UpdateBranchCommandHandler : ICommandHandler<UpdateBranchCommand>
{
    private readonly IBranchRepository _branchRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBranchCommandHandler(IBranchRepository branchRepository, IUnitOfWork unitOfWork)
    {
        _branchRepository = branchRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateBranchCommand request, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.GetByIdAsync(request.Id, cancellationToken);

        if (branch is null)
        {
            return Result.Failure(new Error("Branch.NotFound", "The specified branch does not exist."));
        }

        branch.Update(request.Name, request.Location);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
