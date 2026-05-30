using LoanSystem.Application.Abstractions.Messaging;

namespace LoanSystem.Application.Customers.Referees;

public sealed record AddRefereeCommand(
    Guid CustomerId,
    string Name,
    string Phone,
    string PhysicalAddress,
    string Relationship) : ICommand<Guid>;
