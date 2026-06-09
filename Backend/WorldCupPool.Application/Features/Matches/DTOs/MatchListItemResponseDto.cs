namespace WorldCupPool.Application.Features.Matches.DTOs
{
    public sealed class MatchListItemResponseDto
    {
        public Guid Id { get; set; }
        public string GroupName { get; set; } = string.Empty;
        public string HomeTeam { get; set; } = string.Empty;
        public string HomeTeamCode { get; set; } = string.Empty;
        public string AwayTeam { get; set; } = string.Empty;
        public string AwayTeamCode { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public bool HasFinalResult { get; set; }
        public int? HomeGoals { get; set; }
        public int? AwayGoals { get; set; }
        public DateTime StartTimeUtc { get; set; }
    }
}
