using Moq;
using WorldCupPool.Application.Features.Leaderboard;
using WorldCupPool.Domain.Entities;

namespace WorldCupPool.Tests.Leaderboard;

public sealed class LeaderboardServiceTests
{
    [Fact]
    public async Task GetAsync_ShouldReturnMappedLeaderboardStats()
    {
        var repository = new Mock<IAppUserRepository>();
        var leaderboard = new List<LeaderboardUserStats>
        {
            new()
            {
                UserId = Guid.NewGuid(),
                UserName = "user1",
                FirstName = "World",
                LastName = "Cup",
                Points = 7,
                WonCount = 2,
                CorrectOutcomeCount = 1,
                LostCount = 1
            }
        };
        repository
            .Setup(x => x.GetLeaderboardAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(leaderboard);

        var service = new LeaderboardService(repository.Object);

        var result = await service.GetAsync(CancellationToken.None);

        var item = Assert.Single(result);
        Assert.Equal("user1", item.UserName);
        Assert.Equal("World Cup", item.FullName);
        Assert.Equal(7, item.Points);
        Assert.Equal(2, item.WonCount);
        Assert.Equal(1, item.CorrectOutcomeCount);
        Assert.Equal(1, item.LostCount);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnEmptyList_WhenRepositoryHasNoRows()
    {
        var repository = new Mock<IAppUserRepository>();
        repository
            .Setup(x => x.GetLeaderboardAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<LeaderboardUserStats>());

        var service = new LeaderboardService(repository.Object);

        var result = await service.GetAsync(CancellationToken.None);

        Assert.Empty(result);
    }
}
