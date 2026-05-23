namespace LoanSystem.Api.Identity;

public sealed record LoginRequest(string Email, string Password);
public sealed record LoginResponse(string AccessToken, int ExpiresIn);
public sealed record ChangePasswordRequest(string CurrentPassword, string NewPassword);
