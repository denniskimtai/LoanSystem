using LoanSystem.Application.Abstractions.Messaging;

namespace LoanSystem.Application.CRM.Interactions.Delete;

public sealed record DeleteInteractionCommand(Guid Id) : ICommand;
