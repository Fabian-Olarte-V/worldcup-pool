namespace WorldCupPool.Domain.Entities;

public interface IAppUserRepository
{
    Task<AppUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<AppUser?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);

    Task<AppUser?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<LeaderboardUserStats>> GetLeaderboardAsync(CancellationToken cancellationToken = default);

    Task CreateAsync(AppUser user, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
