using WorldCupPool.Application.Features.Leaderboard.DTOs;

namespace WorldCupPool.Application.Features.Leaderboard
{
    public interface ILeaderboardService
    {
        Task<IReadOnlyList<LeaderboardItemResponseDto>> GetAsync(CancellationToken cancellationToken = default);
    }
}
