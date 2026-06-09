using WorldCupPool.Application.Features.Predictions.DTOs;

namespace WorldCupPool.Application.Features.Predictions
{
    public interface IPredictionsService
    {
        Task<IReadOnlyList<PredictionResponseDto>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default);

        Task<PredictionResponseDto> CreateAsync(Guid userId, CreatePredictionRequestDto request, CancellationToken cancellationToken = default);

        Task<PredictionResponseDto> UpdateAsync(Guid userId, Guid predictionId, UpdatePredictionRequestDto request, CancellationToken cancellationToken = default);

        Task DeleteAsync(Guid userId, Guid predictionId, CancellationToken cancellationToken = default);
    }
}

