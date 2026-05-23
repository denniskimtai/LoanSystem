using LoanSystem.Application.Abstractions.Messaging;

namespace LoanSystem.Application.Identity.Me;

public sealed record GetCurrentUserQuery(Guid UserId) : IQuery<UserResponse>;

public sealed record UserResponse(
    Guid Id,
    string Email,
    string FullName,
    string Role,
    Guid BranchId);
