using LoanSystem.Domain.Entities.Identity;

namespace LoanSystem.Application.Abstractions.Repositories;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);
    Task<List<RefreshToken>> GetActiveTokensForUserAsync(Guid userId, CancellationToken cancellationToken = default);
    void Add(RefreshToken refreshToken);
    void Remove(RefreshToken refreshToken);
}
