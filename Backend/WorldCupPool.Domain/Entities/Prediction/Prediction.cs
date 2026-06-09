using WorldCupPool.Domain.Common;
using WorldCupPool.Domain.Enums;

namespace WorldCupPool.Domain.Entities
{
    public sealed class Prediction : Entity<Guid>
    {
        public Guid UserId { get; private set; }
        public Guid MatchId { get; private set; }
        public int HomeGoals { get; private set; }
        public int AwayGoals { get; private set; }
        public int? Points { get; private set; }
        public PredictionStatus Status { get; private set; } = PredictionStatus.Pending;
        public DateTime CreatedAtUtc { get; private set; }
        public DateTime? UpdatedAtUtc { get; private set; }
        public AppUser User { get; private set; } = null!;
        public Match Match { get; private set; } = null!;

        private Prediction() { }

        public Prediction(Guid userId, Guid matchId, int homeGoals, int awayGoals)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            MatchId = matchId;
            HomeGoals = homeGoals;
            AwayGoals = awayGoals;
            CreatedAtUtc = DateTime.UtcNow;
        }

        public void UpdateScore(int homeGoals, int awayGoals)
        {
            HomeGoals = homeGoals;
            AwayGoals = awayGoals;
            Points = null;
            Status = PredictionStatus.Pending;
            UpdatedAtUtc = DateTime.UtcNow;
        }


        public void UpdatePoints(int points)
        {
            Points = points;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void CalculatePoints(int actualHomeGoals, int actualAwayGoals)
        {
            var points = 0;
            var status = PredictionStatus.Lost;

            if (HomeGoals == actualHomeGoals && AwayGoals == actualAwayGoals)
            {
                points = 3;
                status = PredictionStatus.Won;
            }
            else
            {
                var predictedOutcome = GetOutcome(HomeGoals, AwayGoals);
                var actualOutcome = GetOutcome(actualHomeGoals, actualAwayGoals);

                if (predictedOutcome == actualOutcome)
                {
                    points = 1;
                    status = PredictionStatus.CorrectOutcome;
                }
            }

            UpdatePoints(points);
            Status = status;
        }

        private static MatchOutCome GetOutcome(int homeGoals, int awayGoals)
        {
            if (homeGoals > awayGoals)
            {
                return MatchOutCome.HomeWin;
            }

            if (awayGoals > homeGoals)
            {
                return MatchOutCome.AwayWin;
            }

            return MatchOutCome.Draw;
        }
    }
}


