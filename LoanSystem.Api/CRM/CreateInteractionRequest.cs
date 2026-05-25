using LoanSystem.Domain.Enums;

namespace LoanSystem.Api.CRM;

public sealed record CreateInteractionRequest(
    Guid CustomerId,
    InteractionMode Mode,
    string Purpose,
    string OutcomeDetails,
    InteractionOutcome OutcomeStatus,
    InteractionTag Tag,
    string DefaultReason,
    string NextSteps,
    string LocationGeo,
    DateTime InteractionAt,
    Guid? LoanId = null,
    decimal? PromisedAmount = null,
    DateOnly? NextInteractionDate = null);
