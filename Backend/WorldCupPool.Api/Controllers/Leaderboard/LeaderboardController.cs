using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorldCupPool.Application.Features.Leaderboard;
using WorldCupPool.Application.Features.Leaderboard.DTOs;
using WorldCupPool.Infrastructure.Auth;

namespace WorldCupPool.Api.Controllers.Leaderboard
{
    [ApiController]
    [Route("api/leaderboard")]
    [Authorize(AuthPolicies.UserOrAdmin)]
    public sealed class LeaderboardController : ControllerBase
    {
        private readonly ILeaderboardService _leaderboardService;

        public LeaderboardController(ILeaderboardService leaderboardService)
        {
            _leaderboardService = leaderboardService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<LeaderboardItemResponseDto>>> Get(CancellationToken cancellationToken)
        {
            var result = await _leaderboardService.GetAsync(cancellationToken);
            return Ok(result);
        }
    }
}
