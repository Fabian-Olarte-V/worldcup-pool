using System.Reflection;
using Moq;
using WorldCupPool.Application.Features.Matches;
using WorldCupPool.Application.Features.Matches.DTOs;
using WorldCupPool.Domain.Entities;
using WorldCupPool.Domain.Enums;
using MatchEntity = WorldCupPool.Domain.Entities.Match;

namespace WorldCupPool.Tests.Matches;

public sealed class MatchesServiceTests
{
    [Fact]
    public async Task UpdateResultAsync_ShouldSetFinalResult_AndRecalculatePredictionPoints()
    {
        var homeTeam = new SoccerTeam("Colombia", "co", "A");
        var awayTeam = new SoccerTeam("Portugal", "pt", "A");
        var match = new MatchEntity("A", homeTeam, awayTeam, DateTime.UtcNow, MatchStatus.Scheduled);
        var prediction = new Prediction(Guid.NewGuid(), match.Id, 2, 1);
        AddPrediction(match, prediction);

        var matchRepository = new Mock<IMatchRepository>();
        var matchResultRepository = new Mock<IMatchResultRepository>();
        matchRepository
            .Setup(x => x.GetByIdAsync(match.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(match);

        var service = new MatchesService(matchRepository.Object, matchResultRepository.Object);

        var request = new UpdateMatchResultRequestDto
        {
            MatchId = match.Id,
            HomeGoals = 2,
            AwayGoals = 1
        };

        var response = await service.UpdateResultAsync(request, CancellationToken.None);

        Assert.True(match.HasFinalResult);
        Assert.Equal(MatchStatus.Finished.ToString(), response.Status);
        Assert.Equal(3, prediction.Points);
        Assert.Equal(PredictionStatus.Won, prediction.Status);
        matchResultRepository.Verify(x => x.AddAsync(It.IsAny<MatchResult>(), It.IsAny<CancellationToken>()), Times.Once);
        matchRepository.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateResultAsync_ShouldThrowNotFoundException_WhenMatchDoesNotExist()
    {
        var matchRepository = new Mock<IMatchRepository>();
        var matchResultRepository = new Mock<IMatchResultRepository>();

        matchRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((MatchEntity?)null);

        var service = new MatchesService(matchRepository.Object, matchResultRepository.Object);

        var exception = await Assert.ThrowsAsync<WorldCupPool.Application.Exceptions.NotFoundException>(() =>
            service.UpdateResultAsync(new UpdateMatchResultRequestDto
            {
                MatchId = Guid.NewGuid(),
                HomeGoals = 1,
                AwayGoals = 0
            }, CancellationToken.None));

        Assert.Equal("Match was not found.", exception.Message);
        matchRepository.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateResultAsync_ShouldThrowBusinessRuleViolation_WhenMatchIsAlreadyFinished()
    {
        var homeTeam = new SoccerTeam("Colombia", "co", "A");
        var awayTeam = new SoccerTeam("Portugal", "pt", "A");
        var match = new MatchEntity("A", homeTeam, awayTeam, DateTime.UtcNow, MatchStatus.Finished);

        var matchRepository = new Mock<IMatchRepository>();
        var matchResultRepository = new Mock<IMatchResultRepository>();
        matchRepository
            .Setup(x => x.GetByIdAsync(match.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(match);

        var service = new MatchesService(matchRepository.Object, matchResultRepository.Object);

        var exception = await Assert.ThrowsAsync<WorldCupPool.Application.Exceptions.BusinessRuleViolationException>(() =>
            service.UpdateResultAsync(new UpdateMatchResultRequestDto
            {
                MatchId = match.Id,
                HomeGoals = 2,
                AwayGoals = 1
            }, CancellationToken.None));

        Assert.Equal("Results cannot be updated for finished matches.", exception.Message);
        matchResultRepository.Verify(x => x.AddAsync(It.IsAny<MatchResult>(), It.IsAny<CancellationToken>()), Times.Never);
        matchRepository.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    private static void AddPrediction(MatchEntity match, Prediction prediction)
    {
        var field = typeof(MatchEntity).GetField("_predictions", BindingFlags.Instance | BindingFlags.NonPublic);
        var predictions = Assert.IsType<List<Prediction>>(field?.GetValue(match));
        predictions.Add(prediction);
    }
}
