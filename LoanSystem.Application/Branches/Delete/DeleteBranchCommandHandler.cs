using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Domain.Primitives;

namespace LoanSystem.Application.Branches.Delete;

public sealed class DeleteBranchCommandHandler : ICommandHandler<DeleteBranchCommand>
{
    private readonly IBranchRepository _branchRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBranchCommandHandler(IBranchRepository branchRepository, IUnitOfWork unitOfWork)
    {
        _branchRepository = branchRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteBranchCommand request, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.GetByIdAsync(request.Id, cancellationToken);

        if (branch is null)
        {
            return Result.Failure(new Error("Branch.NotFound", "The specified branch does not exist."));
        }

        branch.MarkAsDeleted();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
