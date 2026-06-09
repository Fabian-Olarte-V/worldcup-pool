namespace WorldCupPool.Application.Features.Matches.DTOs
{
    public sealed class UpdateMatchResultRequestDto
    {
        public Guid MatchId { get; set; }
        public int HomeGoals { get; set; }
        public int AwayGoals { get; set; }
    }
}

