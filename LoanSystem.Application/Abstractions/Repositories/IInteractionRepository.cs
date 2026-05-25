using LoanSystem.Domain.Entities.CRM;
using LoanSystem.Domain.Enums;

namespace LoanSystem.Application.Abstractions.Repositories;

public interface IInteractionRepository
{
    Task<Interaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Add(Interaction interaction);
    Task<(IReadOnlyCollection<Interaction> Items, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        Guid? customerId = null,
        Guid? agentId = null,
        InteractionTag? tag = null,
        InteractionOutcome? outcomeStatus = null,
        CancellationToken cancellationToken = default);
}
