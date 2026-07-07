using LoanSystem.Domain.Enums;

namespace LoanSystem.Application.CRM.Interactions;

public sealed record InteractionResponse(
    Guid Id,
    Guid CustomerId,
    Guid? LoanId,
    Guid AgentId,
    string Mode,
    string Purpose,
    string OutcomeDetails,
    string OutcomeStatus,
    string Tag,
    decimal? PromisedAmount,
    string DefaultReason,
    string NextSteps,
    string? LocationGeo,
    DateOnly? NextInteractionDate,
    DateTime InteractionAt,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
