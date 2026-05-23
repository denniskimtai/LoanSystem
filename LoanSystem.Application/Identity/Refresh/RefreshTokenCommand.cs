using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Application.Identity.Login;

namespace LoanSystem.Application.Identity.Refresh;

public sealed record RefreshTokenCommand(
    string RefreshToken,
    string? IpAddress) : ICommand<LoginResult>;
