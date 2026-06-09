namespace WorldCupPool.Domain.Entities
{
    public sealed class MatchListItemData
    {
        public Guid Id { get; init; }
        public string GroupName { get; init; } = string.Empty;
        public string HomeTeamName { get; init; } = string.Empty;
        public string HomeTeamCode { get; init; } = string.Empty;
        public string AwayTeamName { get; init; } = string.Empty;
        public string AwayTeamCode { get; init; } = string.Empty;
        public string Status { get; init; } = string.Empty;
        public bool HasFinalResult { get; init; }
        public int? HomeGoals { get; init; }
        public int? AwayGoals { get; init; }
        public DateTime StartTimeUtc { get; init; }
    }
}
