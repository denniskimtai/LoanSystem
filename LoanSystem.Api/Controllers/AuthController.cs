using LoanSystem.Api.Identity;
using LoanSystem.Application.Identity.ChangePassword;
using LoanSystem.Application.Identity.Login;
using LoanSystem.Application.Identity.Logout;
using LoanSystem.Application.Identity.Me;
using LoanSystem.Application.Identity.Refresh;
using LoanSystem.Application.Identity.Register;
using LoanSystem.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LoanSystem.Api.Controllers;

[Route("api/auth")]
public sealed class AuthController : ApiController
{
    public AuthController(ISender sender) : base(sender)
    {
    }

    [HttpPost("register")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var command = new LoginCommand(request.Email, request.Password, GetIpAddress());
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        SetRefreshTokenCookie(result.Value.RefreshToken);

        return Ok(new LoginResponse(result.Value.AccessToken, result.Value.ExpiresIn));
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh(CancellationToken cancellationToken)
    {
        if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken) || string.IsNullOrEmpty(refreshToken))
        {
            return Unauthorized(new { Title = "Unauthorized", Status = StatusCodes.Status401Unauthorized, Detail = "Refresh token is missing." });
        }

        var command = new RefreshTokenCommand(refreshToken, GetIpAddress());
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            if (result.Error.Code == "Identity.InvalidRefreshToken" || result.Error.Code == "Identity.UserNotFoundOrInactive")
            {
                return Unauthorized(new { Title = "Unauthorized", Status = StatusCodes.Status401Unauthorized, Detail = result.Error.Message });
            }

            return HandleFailure(result);
        }

        SetRefreshTokenCookie(result.Value.RefreshToken);

        return Ok(new LoginResponse(result.Value.AccessToken, result.Value.ExpiresIn));
    }

    [HttpPost("logout")]
    [AllowAnonymous]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        if (Request.Cookies.TryGetValue("refreshToken", out var refreshToken) && !string.IsNullOrEmpty(refreshToken))
        {
            var command = new LogoutCommand(refreshToken, GetIpAddress());
            await Sender.Send(command, cancellationToken);
        }

        Response.Cookies.Delete("refreshToken", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Path = "/api/auth"
        });

        return Ok();
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetMe(CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return Unauthorized();
        }

        var query = new GetCurrentUserQuery(userId);
        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return Unauthorized();
        }

        var command = new ChangePasswordCommand(userId, request.CurrentPassword, request.NewPassword);
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok();
    }

    private string? GetIpAddress()
    {
        if (Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedHeader))
        {
            var headerValue = forwardedHeader.FirstOrDefault();
            if (!string.IsNullOrEmpty(headerValue))
            {
                var firstIp = headerValue.Split(',').FirstOrDefault()?.Trim();
                if (!string.IsNullOrEmpty(firstIp))
                {
                    return firstIp.Length > 45 ? firstIp.Substring(0, 45) : firstIp;
                }
            }
        }

        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        return ip != null && ip.Length > 45 ? ip.Substring(0, 45) : ip;
    }

    private void SetRefreshTokenCookie(string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Path = "/api/auth",
            Expires = DateTime.UtcNow.AddDays(7)
        };
        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }
}
