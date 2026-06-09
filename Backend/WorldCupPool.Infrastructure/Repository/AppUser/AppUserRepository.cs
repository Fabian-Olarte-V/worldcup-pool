using Microsoft.EntityFrameworkCore;
using WorldCupPool.Domain.Entities;
using WorldCupPool.Domain.Enums;
using WorldCupPool.Infrastructure.Persistence;

namespace WorldCupPool.Infrastructure.Repository.AppUsers
{
    public sealed class AppUserRepository : IAppUserRepository
    {
        private readonly AppDbContext _context;

        public AppUserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AppUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
        }

        public async Task<AppUser?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.UserName == userName, cancellationToken);
        }

        public async Task<AppUser?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.RefreshToken == refreshToken, cancellationToken);
        }

        public async Task<IReadOnlyList<LeaderboardUserStats>> GetLeaderboardAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .Select(x => new LeaderboardUserStats
                {
                    UserId = x.Id,
                    UserName = x.UserName,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Points = x.Predictions.Sum(p => p.Points ?? 0),
                    WonCount = x.Predictions.Count(p => p.Status == PredictionStatus.Won),
                    CorrectOutcomeCount = x.Predictions.Count(p => p.Status == PredictionStatus.CorrectOutcome),
                    LostCount = x.Predictions.Count(p => p.Status == PredictionStatus.Lost)
                })
                .OrderByDescending(x => x.Points)
                .ThenByDescending(x => x.WonCount)
                .ThenByDescending(x => x.CorrectOutcomeCount)
                .ThenBy(x => x.UserName)
                .ToListAsync(cancellationToken);
        }

        public async Task CreateAsync(AppUser user, CancellationToken cancellationToken = default)
        {
            await _context.Users.AddAsync(user, cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
