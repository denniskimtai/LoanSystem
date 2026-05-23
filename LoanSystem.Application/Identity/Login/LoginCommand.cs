using LoanSystem.Application.Abstractions.Messaging;

namespace LoanSystem.Application.Identity.Login;

public sealed record LoginCommand(
    string Email,
    string Password,
    string? IpAddress) : ICommand<LoginResult>;

public sealed record LoginResult(
    string AccessToken,
    string RefreshToken,
    int ExpiresIn);
