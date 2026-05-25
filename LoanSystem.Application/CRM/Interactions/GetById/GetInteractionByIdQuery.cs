using LoanSystem.Application.Abstractions.Messaging;

namespace LoanSystem.Application.CRM.Interactions.GetById;

public sealed record GetInteractionByIdQuery(Guid Id) : IQuery<InteractionResponse>;
