using LoanSystem.Application.Abstractions.Messaging;

namespace LoanSystem.Application.Branches.Delete;

public sealed record DeleteBranchCommand(Guid Id) : ICommand;
