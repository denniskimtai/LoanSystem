using LoanSystem.Application.Abstractions.Messaging;

namespace LoanSystem.Application.Loans.Reject;

public sealed record RejectLoanCommand(Guid Id, Guid UserId) : ICommand;
