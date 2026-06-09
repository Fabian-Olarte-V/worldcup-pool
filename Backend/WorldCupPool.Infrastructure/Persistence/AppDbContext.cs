using Microsoft.EntityFrameworkCore;
using WorldCupPool.Domain.Entities;

namespace WorldCupPool.Infrastructure.Persistence
{
    public sealed class AppDbContext : DbContext
    {
        public DbSet<AppUser> Users => Set<AppUser>();
        public DbSet<Match> Matches => Set<Match>();
        public DbSet<Prediction> Predictions => Set<Prediction>();
        public DbSet<MatchResult> MatchResults => Set<MatchResult>();
        public DbSet<SoccerTeam> SoccerTeams => Set<SoccerTeam>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}