using LoanSystem.Application.Abstractions.Messaging;

namespace LoanSystem.Application.Customers.Referees;

public sealed record RemoveRefereeCommand(
    Guid CustomerId,
    Guid RefereeId) : ICommand;
