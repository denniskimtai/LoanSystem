using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Domain.Enums;
using LoanSystem.Domain.Primitives;

namespace LoanSystem.Application.Loans.Delete;

public sealed class DeleteLoanCommandHandler : ICommandHandler<DeleteLoanCommand>
{
    private readonly ILoanRepository _loanRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteLoanCommandHandler(ILoanRepository loanRepository, IUnitOfWork unitOfWork)
    {
        _loanRepository = loanRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteLoanCommand request, CancellationToken cancellationToken)
    {
        // 1. Get Loan
        var loan = await _loanRepository.GetByIdAsync(request.Id, cancellationToken);
        if (loan is null)
        {
            return Result.Failure(new Error("Loan.NotFound", "The specified loan does not exist."));
        }

        // 2. Validate current state (only allow deletion if status is Created and stage is Initiation)
        if (loan.Status != LoanStatus.Created || loan.Stage != LoanStage.Initiation)
        {
            return Result.Failure(new Error("Loan.DeleteBlocked", "A loan can only be deleted if its status is Created and stage is Initiation."));
        }

        // 3. Perform soft delete
        loan.MarkAsDeleted();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
