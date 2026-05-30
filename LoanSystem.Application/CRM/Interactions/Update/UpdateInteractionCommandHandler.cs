using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Domain.Primitives;

namespace LoanSystem.Application.CRM.Interactions.Update;

public sealed class UpdateInteractionCommandHandler : ICommandHandler<UpdateInteractionCommand>
{
    private readonly IInteractionRepository _interactionRepository;
    private readonly ILoanRepository _loanRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateInteractionCommandHandler(
        IInteractionRepository interactionRepository,
        ILoanRepository loanRepository,
        IUnitOfWork unitOfWork)
    {
        _interactionRepository = interactionRepository;
        _loanRepository = loanRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateInteractionCommand request, CancellationToken cancellationToken)
    {
        // 1. Get interaction
        var interaction = await _interactionRepository.GetByIdAsync(request.Id, cancellationToken);
        if (interaction is null)
        {
            return Result.Failure(new Error("Interaction.NotFound", "The specified interaction does not exist."));
        }

        // 2. Validate Loan if provided
        if (request.LoanId.HasValue)
        {
            var loanExists = await _loanRepository.ExistsAsync(request.LoanId.Value, cancellationToken);
            if (!loanExists)
            {
                return Result.Failure(new Error("Loan.NotFound", "The specified loan does not exist."));
            }
        }

        // 3. Update interaction properties
        interaction.Update(
            request.Mode,
            request.Purpose,
            request.OutcomeDetails,
            request.OutcomeStatus,
            request.Tag,
            request.DefaultReason,
            request.NextSteps,
            request.LocationGeo,
            request.InteractionAt);

        // 4. Update optional properties
        interaction.SetLoan(request.LoanId);
        interaction.SetPromisedAmount(request.PromisedAmount);
        interaction.SetNextInteractionDate(request.NextInteractionDate);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
