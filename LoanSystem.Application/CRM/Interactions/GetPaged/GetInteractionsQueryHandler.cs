using LoanSystem.Application.Abstractions;
using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Domain.Primitives;

namespace LoanSystem.Application.CRM.Interactions.GetPaged;

public sealed class GetInteractionsQueryHandler : IQueryHandler<GetInteractionsQuery, PagedResult<InteractionResponse>>
{
    private readonly IInteractionRepository _interactionRepository;

    public GetInteractionsQueryHandler(IInteractionRepository interactionRepository)
    {
        _interactionRepository = interactionRepository;
    }

    public async Task<Result<PagedResult<InteractionResponse>>> Handle(GetInteractionsQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _interactionRepository.GetPagedAsync(
            request.Page,
            request.PageSize,
            request.CustomerId,
            request.AgentId,
            request.Tag,
            request.OutcomeStatus,
            cancellationToken);

        var mappedItems = items.Select(i => new InteractionResponse(
            i.Id,
            i.CustomerId,
            i.LoanId,
            i.AgentId,
            i.Mode,
            i.Purpose,
            i.OutcomeDetails,
            i.OutcomeStatus,
            i.Tag,
            i.PromisedAmount,
            i.DefaultReason,
            i.NextSteps,
            i.LocationGeo,
            i.NextInteractionDate,
            i.InteractionAt,
            i.CreatedAt,
            i.UpdatedAt)).ToList();

        var pagedResult = new PagedResult<InteractionResponse>(
            mappedItems,
            request.Page,
            request.PageSize,
            totalCount);

        return Result.Success(pagedResult);
    }
}
