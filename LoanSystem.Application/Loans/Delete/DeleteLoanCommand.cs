using LoanSystem.Application.Abstractions.Messaging;

namespace LoanSystem.Application.Loans.Delete;

public sealed record DeleteLoanCommand(Guid Id) : ICommand;
