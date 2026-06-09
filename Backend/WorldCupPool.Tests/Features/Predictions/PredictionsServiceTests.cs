using Moq;
using WorldCupPool.Application.Exceptions;
using WorldCupPool.Application.Features.Predictions;
using WorldCupPool.Application.Features.Predictions.DTOs;
using WorldCupPool.Domain.Entities;
using WorldCupPool.Domain.Enums;
using MatchEntity = WorldCupPool.Domain.Entities.Match;

namespace WorldCupPool.Tests.Predictions;

public sealed class PredictionsServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldThrowBusinessRuleViolation_WhenMatchIsFinished()
    {
        var repository = new Mock<IPredictionRepository>();
        var match = CreateMatch(MatchStatus.Finished);
        repository
            .Setup(x => x.GetMatchByIdAsync(match.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(match);

        var service = new PredictionsService(repository.Object);
        var request = new CreatePredictionRequestDto
        {
            MatchId = match.Id,
            HomeGoals = 1,
            AwayGoals = 0
        };

        var exception = await Assert.ThrowsAsync<BusinessRuleViolationException>(() =>
            service.CreateAsync(Guid.NewGuid(), request, CancellationToken.None));

        Assert.Equal("Predictions cannot be created for finished matches.", exception.Message);
        repository.Verify(x => x.CreateAsync(It.IsAny<Prediction>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowNotFoundException_WhenPredictionDoesNotExist()
    {
        var repository = new Mock<IPredictionRepository>();
        repository
            .Setup(x => x.GetByIdAndUserIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Prediction?)null);

        var service = new PredictionsService(repository.Object);

        var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
            service.UpdateAsync(Guid.NewGuid(), Guid.NewGuid(), new UpdatePredictionRequestDto
            {
                HomeGoals = 1,
                AwayGoals = 1
            }, CancellationToken.None));

        Assert.Equal("Prediction was not found.", exception.Message);
        repository.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    private static MatchEntity CreateMatch(MatchStatus status)
    {
        return new MatchEntity(
            "A",
            new SoccerTeam("Colombia", "co", "A"),
            new SoccerTeam("Portugal", "pt", "A"),
            DateTime.UtcNow,
            status);
    }
}
