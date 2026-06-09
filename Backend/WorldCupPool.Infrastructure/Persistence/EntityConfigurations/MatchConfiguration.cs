using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorldCupPool.Domain.Entities;

namespace WorldCupPool.Infrastructure.Persistence.Configurations
{
    public sealed class MatchConfiguration : IEntityTypeConfiguration<Match>
    {
        public void Configure(EntityTypeBuilder<Match> builder)
        {
            builder.ToTable("Matches");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.GroupName)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(x => x.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(x => x.StartTimeUtc)
                .IsRequired();

            builder.Property(x => x.CreatedAtUtc)
                .IsRequired();

            builder.Property<Guid>("HomeTeamId")
                .IsRequired();

            builder.Property<Guid>("AwayTeamId")
                .IsRequired();

            builder.HasOne(x => x.HomeTeam)
                .WithMany(x => x.HomeMatches)
                .HasForeignKey("HomeTeamId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.AwayTeam)
                .WithMany(x => x.AwayMatches)
                .HasForeignKey("AwayTeamId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Result)
                .WithOne(x => x.Match)
                .HasForeignKey<MatchResult>(x => x.MatchId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Metadata.FindNavigation(nameof(Match.Predictions))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}