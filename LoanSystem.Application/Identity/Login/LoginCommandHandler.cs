using LoanSystem.Application.Abstractions.Identity;
using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Domain.Entities.Identity;
using LoanSystem.Domain.Primitives;
using Microsoft.AspNetCore.Identity;

namespace LoanSystem.Application.Identity.Login;

public sealed class LoginCommandHandler : ICommandHandler<LoginCommand, LoginResult>
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtProvider _jwtProvider;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LoginCommandHandler(
        UserManager<User> userManager,
        IJwtProvider jwtProvider,
        IRefreshTokenRepository refreshTokenRepository,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _jwtProvider = jwtProvider;
        _refreshTokenRepository = refreshTokenRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<LoginResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // 1. Find User by Email
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Result.Failure<LoginResult>(new Error("Identity.InvalidCredentials", "Invalid email or password."));
        }

        // 2. Validate Password
        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid)
        {
            return Result.Failure<LoginResult>(new Error("Identity.InvalidCredentials", "Invalid email or password."));
        }

        // 3. Check Active Status
        if (!user.IsActive)
        {
            return Result.Failure<LoginResult>(new Error("Identity.InactiveUser", "Your account has been deactivated."));
        }

        // 4. Generate Access Token
        var accessToken = _jwtProvider.GenerateToken(user);
        const int expiresInSeconds = 900; // 15 minutes

        // 5. Generate Refresh Token
        var refreshTokenValue = Guid.NewGuid().ToString();
        var expiresAt = DateTime.UtcNow.AddDays(7);

        var refreshToken = new RefreshToken(
            user.Id,
            refreshTokenValue,
            expiresAt,
            request.IpAddress);

        _refreshTokenRepository.Add(refreshToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // 6. Return Result
        return Result.Success(new LoginResult(accessToken, refreshTokenValue, expiresInSeconds));
    }
}
