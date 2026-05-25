using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Domain.Entities.Identity;
using LoanSystem.Domain.Primitives;

namespace LoanSystem.Application.Branches.Create;

public sealed class CreateBranchCommandHandler : ICommandHandler<CreateBranchCommand, Guid>
{
    private readonly IBranchRepository _branchRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBranchCommandHandler(IBranchRepository branchRepository, IUnitOfWork unitOfWork)
    {
        _branchRepository = branchRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateBranchCommand request, CancellationToken cancellationToken)
    {
        var branch = new Branch(request.Name, request.Location);

        _branchRepository.Add(branch);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(branch.Id);
    }
}
