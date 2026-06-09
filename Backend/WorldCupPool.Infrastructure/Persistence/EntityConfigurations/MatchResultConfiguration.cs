using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorldCupPool.Domain.Entities;

namespace WorldCupPool.Infrastructure.Persistence.Configurations
{
    public sealed class MatchResultConfiguration : IEntityTypeConfiguration<MatchResult>
    {
        public void Configure(EntityTypeBuilder<MatchResult> builder)
        {
            builder.ToTable("MatchResults");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.HomeGoals)
                .IsRequired();

            builder.Property(x => x.AwayGoals)
                .IsRequired();

            builder.Property(x => x.CreatedAtUtc)
                .IsRequired();

            builder.Property(x => x.UpdatedAtUtc);

            builder.HasIndex(x => x.MatchId)
                .IsUnique();
        }
    }
}