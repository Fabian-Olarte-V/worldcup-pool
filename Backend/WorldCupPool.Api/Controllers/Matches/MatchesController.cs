using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorldCupPool.Application.Features.Matches;
using WorldCupPool.Application.Features.Matches.DTOs;
using WorldCupPool.Infrastructure.Auth;

namespace WorldCupPool.Api.Controllers.Matches
{
    [ApiController]
    [Route("api/matches")]
    public sealed class MatchesController : ControllerBase
    {
        private readonly IMatchesService _matchesService;

        public MatchesController(IMatchesService matchesService)
        {
            _matchesService = matchesService;
        }

        [HttpGet]
        [Authorize(AuthPolicies.UserOrAdmin)]
        public async Task<ActionResult<IReadOnlyList<MatchListItemResponseDto>>> GetAll(CancellationToken cancellationToken)
        {
            var result = await _matchesService.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        [HttpPost("results")]
        [Authorize(AuthPolicies.Admin)]
        public async Task<ActionResult<MatchListItemResponseDto>> UpdateResult(UpdateMatchResultRequestDto request, CancellationToken cancellationToken)
        {
            var result = await _matchesService.UpdateResultAsync(request, cancellationToken);
            return Ok(result);
        }

        [HttpPost("results/bulk")]
        [Authorize(AuthPolicies.Admin)]
        public async Task<ActionResult<IReadOnlyList<MatchListItemResponseDto>>> UpdateResultsBulk(BulkUpdateMatchResultRequestDto request, CancellationToken cancellationToken)
        {
            var result = await _matchesService.UpdateResultsBulkAsync(request, cancellationToken);
            return Ok(result);
        }
    }
}


