using LoanSystem.Application.Abstractions.Messaging;

namespace LoanSystem.Application.Identity.Register;

public sealed record RegisterCommand(
    string Email,
    string Password,
    string FullName,
    string Role,
    Guid BranchId) : ICommand;
