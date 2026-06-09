namespace WorldCupPool.Application.Features.Predictions.DTOs
{
    public sealed class CreatePredictionRequestDto
    {
        public Guid MatchId { get; set; }
        public int HomeGoals { get; set; }
        public int AwayGoals { get; set; }
    }
}

