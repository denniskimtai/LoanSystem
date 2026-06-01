using LoanSystem.Application.Abstractions.Messaging;

namespace LoanSystem.Application.Users.Get;

public sealed record GetUsersQuery(string? Role) : IQuery<IReadOnlyCollection<UserResponse>>;
