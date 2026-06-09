using Microsoft.EntityFrameworkCore;
using WorldCupPool.Domain.Entities;
using WorldCupPool.Infrastructure.Persistence;

namespace WorldCupPool.Infrastructure.Repository.Predictions
{
    public sealed class PredictionRepository : IPredictionRepository
    {
        private readonly AppDbContext _context;

        public PredictionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Prediction>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.Predictions
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAtUtc)
                .ToListAsync(cancellationToken);
        }

        public async Task<Prediction?> GetByIdAndUserIdAsync(Guid predictionId, Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.Predictions
                .Include(x => x.Match)
                .ThenInclude(x => x.Result)
                .FirstOrDefaultAsync(x => x.Id == predictionId && x.UserId == userId, cancellationToken);
        }

        public async Task<Match?> GetMatchByIdAsync(Guid matchId, CancellationToken cancellationToken = default)
        {
            return await _context.Matches
                .Include(x => x.Result)
                .FirstOrDefaultAsync(x => x.Id == matchId, cancellationToken);
        }

        public async Task CreateAsync(Prediction prediction, CancellationToken cancellationToken = default)
        {
            await _context.Predictions.AddAsync(prediction, cancellationToken);
        }

        public void Remove(Prediction prediction)
        {
            _context.Predictions.Remove(prediction);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}


