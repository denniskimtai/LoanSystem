using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Domain.Primitives;

namespace LoanSystem.Application.CRM.Interactions.GetById;

public sealed class GetInteractionByIdQueryHandler : IQueryHandler<GetInteractionByIdQuery, InteractionResponse>
{
    private readonly IInteractionRepository _interactionRepository;

    public GetInteractionByIdQueryHandler(IInteractionRepository interactionRepository)
    {
        _interactionRepository = interactionRepository;
    }

    public async Task<Result<InteractionResponse>> Handle(GetInteractionByIdQuery request, CancellationToken cancellationToken)
    {
        var interaction = await _interactionRepository.GetByIdAsync(request.Id, cancellationToken);
        if (interaction is null)
        {
            return Result.Failure<InteractionResponse>(new Error("Interaction.NotFound", "The specified interaction does not exist."));
        }

        var response = new InteractionResponse(
            interaction.Id,
            interaction.CustomerId,
            interaction.LoanId,
            interaction.AgentId,
            interaction.Mode,
            interaction.Purpose,
            interaction.OutcomeDetails,
            interaction.OutcomeStatus,
            interaction.Tag,
            interaction.PromisedAmount,
            interaction.DefaultReason,
            interaction.NextSteps,
            interaction.LocationGeo,
            interaction.NextInteractionDate,
            interaction.InteractionAt,
            interaction.CreatedAt,
            interaction.UpdatedAt);

        return Result.Success(response);
    }
}
