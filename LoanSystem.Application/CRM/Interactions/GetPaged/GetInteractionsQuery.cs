using LoanSystem.Application.Abstractions;
using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Domain.Enums;

namespace LoanSystem.Application.CRM.Interactions.GetPaged;

public sealed record GetInteractionsQuery(
    int Page,
    int PageSize,
    Guid? CustomerId = null,
    Guid? AgentId = null,
    string? Tag = null,
    string? OutcomeStatus = null) : IQuery<PagedResult<InteractionResponse>>;
