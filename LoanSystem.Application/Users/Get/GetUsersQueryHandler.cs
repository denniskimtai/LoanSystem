using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Domain.Entities.Identity;
using LoanSystem.Domain.Primitives;
using Microsoft.AspNetCore.Identity;
namespace LoanSystem.Application.Users.Get;

public sealed class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, IReadOnlyCollection<UserResponse>>
{
    private readonly UserManager<User> _userManager;

    public GetUsersQueryHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<IReadOnlyCollection<UserResponse>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(request.Role))
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(request.Role);
            var response = usersInRole
                .Where(u => u.IsActive)
                .Select(u => new UserResponse(u.Id, u.FullName, u.Email ?? ""))
                .ToList();
            
            return Result.Success<IReadOnlyCollection<UserResponse>>(response);
        }

        var allUsers = _userManager.Users
            .Where(u => u.IsActive)
            .ToList();

        var allUsersResponse = allUsers
            .Select(u => new UserResponse(u.Id, u.FullName, u.Email ?? ""))
            .ToList();

        return Result.Success<IReadOnlyCollection<UserResponse>>(allUsersResponse);
    }
}
