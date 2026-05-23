using LoanSystem.Domain.Entities.Identity;

namespace LoanSystem.Application.Abstractions.Identity;

public interface IJwtProvider
{
    string GenerateToken(User user);
}
