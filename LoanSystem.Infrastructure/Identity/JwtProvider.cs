using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LoanSystem.Application.Abstractions.Identity;
using LoanSystem.Domain.Entities.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LoanSystem.Infrastructure.Identity;

public sealed class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _options;

    public JwtProvider(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new Claim("fullName", user.FullName),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim("branchId", user.BranchId.ToString())
        };

        var keyBytes = Encoding.UTF8.GetBytes(_options.SecretKey);
        var signingKeyBytes = keyBytes.Length < 32 
            ? System.Security.Cryptography.SHA256.HashData(keyBytes) 
            : keyBytes;
        var signingKey = new SymmetricSecurityKey(signingKeyBytes);
        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _options.Issuer,
            _options.Audience,
            claims,
            null,
            DateTime.UtcNow.AddMinutes(15),
            signingCredentials);

        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenValue;
    }
}
