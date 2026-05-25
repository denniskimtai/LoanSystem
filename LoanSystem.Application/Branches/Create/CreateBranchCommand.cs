using LoanSystem.Application.Abstractions.Messaging;

namespace LoanSystem.Application.Branches.Create;

public sealed record CreateBranchCommand(
    string Name,
    string Location) : ICommand<Guid>;
