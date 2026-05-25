using LoanSystem.Application.Abstractions.Messaging;

namespace LoanSystem.Application.Customers.Delete;

public sealed record DeleteCustomerCommand(Guid Id) : ICommand;
