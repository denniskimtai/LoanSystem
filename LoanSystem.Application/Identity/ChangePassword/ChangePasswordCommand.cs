using LoanSystem.Application.Abstractions.Messaging;

namespace LoanSystem.Application.Identity.ChangePassword;

public sealed record ChangePasswordCommand(
    Guid UserId,
    string CurrentPassword,
    string NewPassword) : ICommand;
