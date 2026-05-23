using LoanSystem.Application.Abstractions.Identity;
using LoanSystem.Application.Abstractions.Messaging;
using LoanSystem.Application.Abstractions.Repositories;
using LoanSystem.Application.Identity.Login;
using LoanSystem.Domain.Entities.Identity;
using LoanSystem.Domain.Primitives;
using Microsoft.AspNetCore.Identity;

namespace LoanSystem.Application.Identity.Refresh;

public sealed class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, LoginResult>
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtProvider _jwtProvider;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RefreshTokenCommandHandler(
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

    public async Task<Result<LoginResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        // 1. Fetch Refresh Token from database
        var tokenEntity = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken);
        if (tokenEntity is null)
        {
            return Result.Failure<LoginResult>(new Error("Identity.InvalidRefreshToken", "Invalid refresh token."));
        }

        // 2. Check for Token Re-use (Potential Breach)
        if (!tokenEntity.IsActive)
        {
            if (tokenEntity.RevokedAt is not null)
            {
                // Breach detected! Revoke all active tokens for this user.
                var activeTokens = await _refreshTokenRepository.GetActiveTokensForUserAsync(tokenEntity.UserId, cancellationToken);
                foreach (var activeToken in activeTokens)
                {
                    activeToken.Revoke(request.IpAddress, "Breach detected: Attempted reuse of revoked token.");
                }
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            return Result.Failure<LoginResult>(new Error("Identity.InvalidRefreshToken", "Invalid refresh token."));
        }

        // 3. Find User
        var user = await _userManager.FindByIdAsync(tokenEntity.UserId.ToString());
        if (user is null || !user.IsActive)
        {
            return Result.Failure<LoginResult>(new Error("Identity.UserNotFoundOrInactive", "User is inactive or does not exist."));
        }

        // 4. Rotate Token (Revoke old, issue new)
        var newRefreshTokenValue = Guid.NewGuid().ToString();
        var expiresAt = DateTime.UtcNow.AddDays(7);

        tokenEntity.Revoke(request.IpAddress, newRefreshTokenValue);

        var newRefreshToken = new RefreshToken(
            user.Id,
            newRefreshTokenValue,
            expiresAt,
            request.IpAddress);

        _refreshTokenRepository.Add(newRefreshToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // 5. Generate New Access Token
        var accessToken = _jwtProvider.GenerateToken(user);
        const int expiresInSeconds = 900; // 15 minutes

        return Result.Success(new LoginResult(accessToken, newRefreshTokenValue, expiresInSeconds));
    }
}
