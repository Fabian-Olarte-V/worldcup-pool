namespace WorldCupPool.Domain.Entities
{
    public interface IPredictionRepository
    {
        Task<IReadOnlyList<Prediction>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

        Task<Prediction?> GetByIdAndUserIdAsync(Guid predictionId, Guid userId, CancellationToken cancellationToken = default);

        Task<Match?> GetMatchByIdAsync(Guid matchId, CancellationToken cancellationToken = default);

        Task CreateAsync(Prediction prediction, CancellationToken cancellationToken = default);

        void Remove(Prediction prediction);

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

