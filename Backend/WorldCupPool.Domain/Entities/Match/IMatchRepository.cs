namespace WorldCupPool.Domain.Entities
{
    public interface IMatchRepository
    {
        Task<IReadOnlyList<MatchListItemData>> GetAllListItemsAsync(CancellationToken cancellationToken = default);

        Task<IReadOnlyList<Match>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<Match?> GetByIdAsync(Guid matchId, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<Match>> GetByIdsAsync(IEnumerable<Guid> matchIds, CancellationToken cancellationToken = default);

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
