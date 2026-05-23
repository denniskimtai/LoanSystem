using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Domain.Entities.Identity;
using LoanSystem.Domain.Enums;
using LoanSystem.Domain.Primitives;
using Microsoft.AspNetCore.Identity;

namespace LoanSystem.Application.Identity.Register;

public sealed class RegisterCommandHandler : ICommandHandler<RegisterCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly IBranchRepository _branchRepository;

    public RegisterCommandHandler(
        UserManager<User> userManager,
        IBranchRepository branchRepository)
    {
        _userManager = userManager;
        _branchRepository = branchRepository;
    }

    public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // 1. Validate Role
        if (!Enum.TryParse<UserRole>(request.Role, true, out var userRole))
        {
            return Result.Failure(new Error("Identity.InvalidRole", "The specified role is invalid."));
        }

        // 2. Validate Branch
        var branch = await _branchRepository.GetByIdAsync(request.BranchId, cancellationToken);
        if (branch is null)
        {
            return Result.Failure(new Error("Identity.BranchNotFound", "The specified branch does not exist."));
        }

        // 3. Check duplicate email
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser is not null)
        {
            return Result.Failure(new Error("Identity.DuplicateEmail", "The specified email is already in use."));
        }

        // 4. Create User entity
        var user = new User(request.Email, request.FullName, userRole, request.BranchId);

        // 5. Save User
        var identityResult = await _userManager.CreateAsync(user, request.Password);
        if (!identityResult.Succeeded)
        {
            var errorDetails = string.Join("; ", identityResult.Errors.Select(e => e.Description));
            return Result.Failure(new Error("Identity.RegistrationFailed", errorDetails));
        }

        return Result.Success();
    }
}
