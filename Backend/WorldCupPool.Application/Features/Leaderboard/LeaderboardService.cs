using WorldCupPool.Application.Features.Leaderboard.DTOs;
using WorldCupPool.Domain.Entities;

namespace WorldCupPool.Application.Features.Leaderboard
{
    public sealed class LeaderboardService : ILeaderboardService
    {
        private readonly IAppUserRepository _appUserRepository;

        public LeaderboardService(IAppUserRepository appUserRepository)
        {
            _appUserRepository = appUserRepository;
        }

        public async Task<IReadOnlyList<LeaderboardItemResponseDto>> GetAsync(CancellationToken cancellationToken = default)
        {
            var users = await _appUserRepository.GetLeaderboardAsync(cancellationToken);
            return users.Select(MapToResponse).ToList();
        }

        private static LeaderboardItemResponseDto MapToResponse(LeaderboardUserStats user)
        {
            return new LeaderboardItemResponseDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                FullName = $"{user.FirstName} {user.LastName}".Trim(),
                Points = user.Points,
                WonCount = user.WonCount,
                CorrectOutcomeCount = user.CorrectOutcomeCount,
                LostCount = user.LostCount
            };
        }
    }
}
