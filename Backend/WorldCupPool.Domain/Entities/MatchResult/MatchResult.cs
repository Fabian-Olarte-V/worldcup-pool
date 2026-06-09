using WorldCupPool.Domain.Common;

namespace WorldCupPool.Domain.Entities
{
    public sealed class MatchResult : Entity<Guid>
    {
        public Guid MatchId { get; private set; }
        public int HomeGoals { get; private set; }
        public int AwayGoals { get; private set; }
        public DateTime CreatedAtUtc { get; private set; }
        public DateTime? UpdatedAtUtc { get; private set; }
        public Match Match { get; private set; } = null!;

        private MatchResult() { }

        public MatchResult(Guid matchId, int homeGoals, int awayGoals)
        {
            Id = Guid.NewGuid();
            MatchId = matchId;
            HomeGoals = homeGoals;
            AwayGoals = awayGoals;
            CreatedAtUtc = DateTime.UtcNow;
        }

        public void Update(int homeGoals, int awayGoals)
        {
            HomeGoals = homeGoals;
            AwayGoals = awayGoals;
            UpdatedAtUtc = DateTime.UtcNow;
        }
    }
}


