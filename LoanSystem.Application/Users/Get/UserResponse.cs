namespace LoanSystem.Application.Users.Get;

public sealed record UserResponse(Guid Id, string FullName, string Email);
