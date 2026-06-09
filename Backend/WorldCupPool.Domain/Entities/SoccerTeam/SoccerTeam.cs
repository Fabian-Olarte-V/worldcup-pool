using WorldCupPool.Domain.Common;

namespace WorldCupPool.Domain.Entities
{
    public sealed class SoccerTeam : Entity<Guid>
    {
        public string Name { get; private set; } = string.Empty;
        public string Code { get; private set; } = string.Empty;
        public string GroupName { get; private set; } = string.Empty;
        public string? FlagUrl { get; private set; }
        public DateTime CreatedAtUtc { get; private set; }
        private readonly List<Match> _homeMatches = [];
        private readonly List<Match> _awayMatches = [];
        public IReadOnlyCollection<Match> HomeMatches => _homeMatches.AsReadOnly();
        public IReadOnlyCollection<Match> AwayMatches => _awayMatches.AsReadOnly();

        private SoccerTeam() { }

        public SoccerTeam(string name, string code, string groupName, string? flagUrl = null)
        {
            Id = Guid.NewGuid();
            Name = name;
            Code = code;
            GroupName = groupName;
            FlagUrl = flagUrl;
            CreatedAtUtc = DateTime.UtcNow;
        }
    }
}


