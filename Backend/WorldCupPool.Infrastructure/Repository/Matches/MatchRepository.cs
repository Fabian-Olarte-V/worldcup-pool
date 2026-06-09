using Microsoft.EntityFrameworkCore;
using WorldCupPool.Domain.Entities;
using WorldCupPool.Infrastructure.Persistence;

namespace WorldCupPool.Infrastructure.Repository.Matches
{
    public sealed class MatchRepository : IMatchRepository
    {
        private readonly AppDbContext _context;

        public MatchRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<MatchListItemData>> GetAllListItemsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Matches
                .AsNoTracking()
                .OrderBy(x => x.StartTimeUtc)
                .Select(x => new MatchListItemData
                {
                    Id = x.Id,
                    GroupName = x.GroupName,
                    HomeTeamName = x.HomeTeam.Name,
                    HomeTeamCode = x.HomeTeam.Code,
                    AwayTeamName = x.AwayTeam.Name,
                    AwayTeamCode = x.AwayTeam.Code,
                    Status = x.Status.ToString(),
                    HasFinalResult = x.Result != null,
                    HomeGoals = x.Result != null ? x.Result.HomeGoals : null,
                    AwayGoals = x.Result != null ? x.Result.AwayGoals : null,
                    StartTimeUtc = x.StartTimeUtc
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Match>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Matches
                .Include(x => x.HomeTeam)
                .Include(x => x.AwayTeam)
                .Include(x => x.Result)
                .OrderBy(x => x.StartTimeUtc)
                .ToListAsync(cancellationToken);
        }

        public async Task<Match?> GetByIdAsync(Guid matchId, CancellationToken cancellationToken = default)
        {
            return await _context.Matches
                .Include(x => x.HomeTeam)
                .Include(x => x.AwayTeam)
                .Include(x => x.Result)
                .Include(x => x.Predictions)
                .FirstOrDefaultAsync(x => x.Id == matchId, cancellationToken);
        }

        public async Task<IReadOnlyList<Match>> GetByIdsAsync(IEnumerable<Guid> matchIds, CancellationToken cancellationToken = default)
        {
            return await _context.Matches
                .Include(x => x.HomeTeam)
                .Include(x => x.AwayTeam)
                .Include(x => x.Result)
                .Include(x => x.Predictions)
                .Where(x => matchIds.Contains(x.Id))
                .ToListAsync(cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
