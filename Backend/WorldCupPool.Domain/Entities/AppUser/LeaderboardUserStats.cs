namespace WorldCupPool.Domain.Entities
{
    public sealed class LeaderboardUserStats
    {
        public Guid UserId { get; init; }
        public string UserName { get; init; } = string.Empty;
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public int Points { get; init; }
        public int WonCount { get; init; }
        public int CorrectOutcomeCount { get; init; }
        public int LostCount { get; init; }
    }
}
