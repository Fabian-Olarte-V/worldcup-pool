namespace WorldCupPool.Application.Features.Predictions.DTOs
{
    public sealed class PredictionResponseDto
    {
        public Guid Id { get; set; }
        public Guid MatchId { get; set; }
        public int HomeGoals { get; set; }
        public int AwayGoals { get; set; }
        public int? Points { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? UpdatedAtUtc { get; set; }
    }
}

