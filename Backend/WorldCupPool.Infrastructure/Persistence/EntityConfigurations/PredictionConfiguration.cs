using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorldCupPool.Domain.Entities;

namespace WorldCupPool.Infrastructure.Persistence.Configurations
{
    public sealed class PredictionConfiguration : IEntityTypeConfiguration<Prediction>
    {
        public void Configure(EntityTypeBuilder<Prediction> builder)
        {
            builder.ToTable("Predictions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.HomeGoals)
                .IsRequired();

            builder.Property(x => x.AwayGoals)
                .IsRequired();

            builder.Property(x => x.Points);

            builder.Property(x => x.Status)
                .IsRequired();

            builder.Property(x => x.CreatedAtUtc)
                .IsRequired();

            builder.Property(x => x.UpdatedAtUtc);

            builder.HasIndex(x => new { x.UserId, x.MatchId })
                .IsUnique();

            builder.HasOne(x => x.User)
                .WithMany(x => x.Predictions)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Match)
                .WithMany(x => x.Predictions)
                .HasForeignKey(x => x.MatchId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
