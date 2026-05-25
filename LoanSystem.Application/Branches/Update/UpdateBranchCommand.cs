using LoanSystem.Application.Abstractions.Messaging;

namespace LoanSystem.Application.Branches.Update;

public sealed record UpdateBranchCommand(
    Guid Id,
    string Name,
    string Location) : ICommand;
