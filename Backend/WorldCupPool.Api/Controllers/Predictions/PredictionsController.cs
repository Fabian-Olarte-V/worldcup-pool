using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorldCupPool.Api.Common.Extensions;
using WorldCupPool.Application.Features.Predictions;
using WorldCupPool.Application.Features.Predictions.DTOs;
using WorldCupPool.Infrastructure.Auth;

namespace WorldCupPool.Api.Controllers.Predictions
{
    [ApiController]
    [Route("api/predictions")]
    [Authorize(AuthPolicies.UserOrAdmin)]
    public sealed class PredictionsController : ControllerBase
    {
        private readonly IPredictionsService _predictionsService;

        public PredictionsController(IPredictionsService predictionsService)
        {
            _predictionsService = predictionsService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<PredictionResponseDto>>> GetPredictions(CancellationToken cancellationToken)
        {
            var result = await _predictionsService.GetByUserAsync(User.GetUserId(), cancellationToken);
            return Ok(result);
        }

        [HttpGet("user/{userId:guid}")]
        [AllowAnonymous]
        public async Task<ActionResult<IReadOnlyList<PredictionResponseDto>>> GetPredictionsByUserId(Guid userId, CancellationToken cancellationToken)
        {
            var result = await _predictionsService.GetByUserAsync(userId, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<PredictionResponseDto>> Create(CreatePredictionRequestDto request, CancellationToken cancellationToken)
        {
            var result = await _predictionsService.CreateAsync(User.GetUserId(), request, cancellationToken);
            return Ok(result);
        }
    }
}

