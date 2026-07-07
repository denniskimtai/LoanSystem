using LoanSystem.Application.Abstractions.Messaging;

namespace LoanSystem.Application.Loans.Approve;

public sealed record ApproveLoanCommand(Guid Id, Guid UserId) : ICommand;
