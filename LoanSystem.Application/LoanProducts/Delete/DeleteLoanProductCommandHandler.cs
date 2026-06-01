using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Domain.Primitives;

namespace LoanSystem.Application.LoanProducts.Delete;

public sealed class DeleteLoanProductCommandHandler : ICommandHandler<DeleteLoanProductCommand>
{
    private readonly ILoanProductRepository _loanProductRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteLoanProductCommandHandler(ILoanProductRepository loanProductRepository, IUnitOfWork unitOfWork)
    {
        _loanProductRepository = loanProductRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteLoanProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _loanProductRepository.GetByIdAsync(request.Id, cancellationToken);
        if (product is null)
        {
            return Result.Failure(new Error("LoanProduct.NotFound", "The specified loan product does not exist."));
        }

        _loanProductRepository.Delete(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
