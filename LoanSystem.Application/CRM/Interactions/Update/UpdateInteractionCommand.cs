using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Domain.Enums;

namespace LoanSystem.Application.CRM.Interactions.Update;

public sealed record UpdateInteractionCommand(
    Guid Id,
    string Mode,
    string Purpose,
    string OutcomeDetails,
    string OutcomeStatus,
    string Tag,
    string DefaultReason,
    string NextSteps,
    string LocationGeo,
    DateTime InteractionAt,
    Guid? LoanId = null,
    decimal? PromisedAmount = null,
    DateOnly? NextInteractionDate = null) : ICommand;
