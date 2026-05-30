using LoanSystem.Domain.Enums;

namespace LoanSystem.Application.CRM.Interactions;

public sealed record InteractionResponse(
    Guid Id,
    Guid CustomerId,
    Guid? LoanId,
    Guid AgentId,
    InteractionMode Mode,
    string Purpose,
    string OutcomeDetails,
    InteractionOutcome OutcomeStatus,
    InteractionTag Tag,
    decimal? PromisedAmount,
    string DefaultReason,
    string NextSteps,
    string LocationGeo,
    DateOnly? NextInteractionDate,
    DateTime InteractionAt,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
