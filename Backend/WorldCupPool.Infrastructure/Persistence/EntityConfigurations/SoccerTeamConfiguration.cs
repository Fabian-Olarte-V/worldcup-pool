using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorldCupPool.Domain.Entities;

namespace WorldCupPool.Infrastructure.Persistence.Configurations
{
    public sealed class SoccerTeamConfiguration : IEntityTypeConfiguration<SoccerTeam>
    {
        public void Configure(EntityTypeBuilder<SoccerTeam> builder)
        {
            builder.ToTable("SoccerTeams");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(x => x.Code)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(x => x.GroupName)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(x => x.FlagUrl)
                .HasMaxLength(500);

            builder.Property(x => x.CreatedAtUtc)
                .IsRequired();

            builder.HasIndex(x => x.Name)
                .IsUnique();

            builder.HasIndex(x => x.Code)
                .IsUnique();

            builder.Metadata.FindNavigation(nameof(SoccerTeam.HomeMatches))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.Metadata.FindNavigation(nameof(SoccerTeam.AwayMatches))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}

