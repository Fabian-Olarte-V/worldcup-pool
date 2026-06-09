namespace WorldCupPool.Domain.Entities;

public interface IMatchResultRepository
{
    Task AddAsync(MatchResult result, CancellationToken cancellationToken = default);
}
