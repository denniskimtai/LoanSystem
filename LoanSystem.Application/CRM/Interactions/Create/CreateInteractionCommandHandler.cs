using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Domain.Entities.CRM;
using LoanSystem.Domain.Primitives;

namespace LoanSystem.Application.CRM.Interactions.Create;

public sealed class CreateInteractionCommandHandler : ICommandHandler<CreateInteractionCommand, Guid>
{
    private readonly IInteractionRepository _interactionRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly ILoanRepository _loanRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateInteractionCommandHandler(
        IInteractionRepository interactionRepository,
        ICustomerRepository customerRepository,
        ILoanRepository loanRepository,
        IUnitOfWork unitOfWork)
    {
        _interactionRepository = interactionRepository;
        _customerRepository = customerRepository;
        _loanRepository = loanRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateInteractionCommand request, CancellationToken cancellationToken)
    {
        // 1. Validate Customer
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);
        if (customer is null)
        {
            return Result.Failure<Guid>(new Error("Customer.NotFound", "The specified customer does not exist."));
        }

        // 2. Validate Loan if provided
        if (request.LoanId.HasValue)
        {
            var loanExists = await _loanRepository.ExistsAsync(request.LoanId.Value, cancellationToken);
            if (!loanExists)
            {
                return Result.Failure<Guid>(new Error("Loan.NotFound", "The specified loan does not exist."));
            }
        }

        // 3. Create Interaction entity
        var interaction = new Interaction(
            request.CustomerId,
            request.AgentId,
            request.Mode,
            request.Purpose,
            request.OutcomeDetails,
            request.OutcomeStatus,
            request.Tag,
            request.DefaultReason,
            request.NextSteps,
            request.LocationGeo,
            request.InteractionAt);

        // 4. Set optional properties
        if (request.LoanId.HasValue)
        {
            interaction.SetLoan(request.LoanId.Value);
        }

        if (request.PromisedAmount.HasValue)
        {
            interaction.SetPromisedAmount(request.PromisedAmount.Value);
        }

        if (request.NextInteractionDate.HasValue)
        {
            interaction.SetNextInteractionDate(request.NextInteractionDate.Value);
        }

        // 5. Add and Save
        _interactionRepository.Add(interaction);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(interaction.Id);
    }
}
