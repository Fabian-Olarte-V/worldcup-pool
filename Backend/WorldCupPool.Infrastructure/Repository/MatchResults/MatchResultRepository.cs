using WorldCupPool.Domain.Entities;
using WorldCupPool.Infrastructure.Persistence;

namespace WorldCupPool.Infrastructure.Repository.MatchResults
{
    public sealed class MatchResultRepository : IMatchResultRepository
    {
        private readonly AppDbContext _context;

        public MatchResultRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(MatchResult result, CancellationToken cancellationToken = default)
        {
            await _context.MatchResults.AddAsync(result, cancellationToken);
        }
    }
}