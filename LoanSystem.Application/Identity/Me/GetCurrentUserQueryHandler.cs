using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Domain.Entities.Identity;
using LoanSystem.Domain.Primitives;
using Microsoft.AspNetCore.Identity;

namespace LoanSystem.Application.Identity.Me;

public sealed class GetCurrentUserQueryHandler : IQueryHandler<GetCurrentUserQuery, UserResponse>
{
    private readonly UserManager<User> _userManager;

    public GetCurrentUserQueryHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<UserResponse>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());

        if (user is null || !user.IsActive)
        {
            return Result.Failure<UserResponse>(new Error("Identity.UserNotFoundOrInactive", "User was not found or is inactive."));
        }

        var response = new UserResponse(
            user.Id,
            user.Email ?? string.Empty,
            user.FullName,
            user.Role.ToString(),
            user.BranchId);

        return Result.Success(response);
    }
}
