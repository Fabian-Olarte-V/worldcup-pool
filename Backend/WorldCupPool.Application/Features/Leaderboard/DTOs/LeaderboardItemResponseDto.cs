namespace WorldCupPool.Application.Features.Leaderboard.DTOs
{
    public sealed class LeaderboardItemResponseDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public int Points { get; set; }
        public int WonCount { get; set; }
        public int CorrectOutcomeCount { get; set; }
        public int LostCount { get; set; }
    }
}
