using LoanSystem.Application.Abstractions.Messaging;

namespace LoanSystem.Application.Identity.Logout;

public sealed record LogoutCommand(
    string RefreshToken,
    string? IpAddress) : ICommand;
