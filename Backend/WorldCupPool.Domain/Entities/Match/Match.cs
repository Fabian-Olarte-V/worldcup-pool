using WorldCupPool.Domain.Common;
using WorldCupPool.Domain.Enums;

namespace WorldCupPool.Domain.Entities
{
    public sealed class Match : Entity<Guid>
    {
        public string GroupName { get; private set; } = string.Empty;
        public SoccerTeam HomeTeam { get; private set; } = null!;
        public SoccerTeam AwayTeam { get; private set; } = null!;
        public MatchStatus Status { get; private set; }
        public MatchResult? Result { get; private set; }
        public DateTime StartTimeUtc { get; private set; }
        public DateTime CreatedAtUtc { get; private set; }
        private readonly List<Prediction> _predictions = [];
        public IReadOnlyCollection<Prediction> Predictions => _predictions.AsReadOnly();
        public bool HasFinalResult => Result is not null;

        private Match() { }

        public Match(
            string groupName,
            SoccerTeam homeTeam,
            SoccerTeam awayTeam,
            DateTime startTimeUtc,
            MatchStatus status = MatchStatus.Scheduled)
        {
            Id = Guid.NewGuid();
            GroupName = groupName;
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
            StartTimeUtc = startTimeUtc;
            Status = status;
            CreatedAtUtc = DateTime.UtcNow;
        }


        public void SetResult(MatchResult result)
        {
            Result = result;
            Status = MatchStatus.Finished;
        }
    }
}

