using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Domain.Entities.Identity;
using LoanSystem.Domain.Primitives;
using Microsoft.AspNetCore.Identity;

namespace LoanSystem.Application.Identity.ChangePassword;

public sealed class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand>
{
    private readonly UserManager<User> _userManager;

    public ChangePasswordCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());

        if (user is null || !user.IsActive)
        {
            return Result.Failure(new Error("Identity.UserNotFoundOrInactive", "User was not found or is inactive."));
        }

        var identityResult = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

        if (!identityResult.Succeeded)
        {
            var errorDetails = string.Join("; ", identityResult.Errors.Select(e => e.Description));
            return Result.Failure(new Error("Identity.ChangePasswordFailed", errorDetails));
        }

        return Result.Success();
    }
}
