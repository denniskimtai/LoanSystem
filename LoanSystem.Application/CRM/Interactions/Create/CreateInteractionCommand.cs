using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Domain.Enums;

namespace LoanSystem.Application.CRM.Interactions.Create;

public sealed record CreateInteractionCommand(
    Guid CustomerId,
    Guid AgentId,
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
    DateOnly? NextInteractionDate = null) : ICommand<Guid>;
