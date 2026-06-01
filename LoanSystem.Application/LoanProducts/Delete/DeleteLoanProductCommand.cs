using LoanSystem.Application.Abstractions.Messaging;

namespace LoanSystem.Application.LoanProducts.Delete;

public sealed record DeleteLoanProductCommand(Guid Id) : ICommand;
