using WorldCupPool.Application.Features.Predictions.DTOs;
using WorldCupPool.Application.Exceptions;
using WorldCupPool.Domain.Entities;

namespace WorldCupPool.Application.Features.Predictions;

public sealed class PredictionsService : IPredictionsService
{
    private readonly IPredictionRepository _predictionRepository;

    public PredictionsService(IPredictionRepository predictionRepository)
    {
        _predictionRepository = predictionRepository;
    }


    public async Task<IReadOnlyList<PredictionResponseDto>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var predictions = await _predictionRepository.GetByUserIdAsync(userId, cancellationToken);
        return predictions.Select(MapToResponse).ToList();
    }

    public async Task<PredictionResponseDto> CreateAsync(Guid userId, CreatePredictionRequestDto request, CancellationToken cancellationToken = default)
    {
        var match = await _predictionRepository.GetMatchByIdAsync(request.MatchId, cancellationToken)
            ?? throw new NotFoundException("Match was not found.");

        if (match.Status == Domain.Enums.MatchStatus.Finished)
        {
            throw new BusinessRuleViolationException("Predictions cannot be created for finished matches.");
        }

        var prediction = new Prediction(userId, match.Id, request.HomeGoals, request.AwayGoals);

        await _predictionRepository.CreateAsync(prediction, cancellationToken);
        await _predictionRepository.SaveChangesAsync(cancellationToken);

        return MapToResponse(prediction);
    }

    public async Task<PredictionResponseDto> UpdateAsync(Guid userId, Guid predictionId, UpdatePredictionRequestDto request, CancellationToken cancellationToken = default)
    {
        var prediction = await _predictionRepository.GetByIdAndUserIdAsync(predictionId, userId, cancellationToken)
            ?? throw new NotFoundException("Prediction was not found.");

        if (prediction.Match.Status == Domain.Enums.MatchStatus.Finished)
        {
            throw new BusinessRuleViolationException("Predictions cannot be updated for finished matches.");
        }

        prediction.UpdateScore(request.HomeGoals, request.AwayGoals);
        await _predictionRepository.SaveChangesAsync(cancellationToken);

        return MapToResponse(prediction);
    }

    public async Task DeleteAsync(Guid userId, Guid predictionId, CancellationToken cancellationToken = default)
    {
        var prediction = await _predictionRepository.GetByIdAndUserIdAsync(predictionId, userId, cancellationToken)
            ?? throw new NotFoundException("Prediction was not found.");

        _predictionRepository.Remove(prediction);
        await _predictionRepository.SaveChangesAsync(cancellationToken);
    }


    private static PredictionResponseDto MapToResponse(Prediction prediction)
    {
        return new PredictionResponseDto
        {
            Id = prediction.Id,
            MatchId = prediction.MatchId,
            HomeGoals = prediction.HomeGoals,
            AwayGoals = prediction.AwayGoals,
            Points = prediction.Points,
            CreatedAtUtc = prediction.CreatedAtUtc,
            UpdatedAtUtc = prediction.UpdatedAtUtc
        };
    }
}
