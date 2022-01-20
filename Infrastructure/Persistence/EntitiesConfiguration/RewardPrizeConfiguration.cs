using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntitiesConfiguration
{
    public class RewardPrizeConfiguration : IEntityTypeConfiguration<RewardPrize>
    {
        public void Configure(EntityTypeBuilder<RewardPrize> builder)
        {
            builder.HasKey(p => new { p.RewardClassId, p.RewardCriteriaId });
            builder.Property(p => p.LeadOnlyPrize).HasColumnType("decimal(8, 2)");
            builder.Property(p => p.LeadWithPOPrize).HasColumnType("decimal(8, 2)");

            builder.HasOne(p => p.RewardClass)
                .WithMany()
                .HasForeignKey(p => p.RewardClassId);

            builder.HasOne(p => p.RewardCriteria)
                .WithMany()
                .HasForeignKey(p => p.RewardCriteriaId);

            builder.HasData(
                new RewardPrize { RewardClassId = 1, RewardCriteriaId = 1, LeadOnlyPrize = 1500, LeadWithPOPrize = 3000 },
                new RewardPrize { RewardClassId = 1, RewardCriteriaId = 2, LeadOnlyPrize = 2000, LeadWithPOPrize = 4000 },
                new RewardPrize { RewardClassId = 1, RewardCriteriaId = 3, LeadOnlyPrize = 2500, LeadWithPOPrize = 5000 },
                new RewardPrize { RewardClassId = 1, RewardCriteriaId = 4, LeadOnlyPrize = 3000, LeadWithPOPrize = 6000 },
                new RewardPrize { RewardClassId = 2, RewardCriteriaId = 1, LeadOnlyPrize = 1000, LeadWithPOPrize = 2000 },
                new RewardPrize { RewardClassId = 2, RewardCriteriaId = 2, LeadOnlyPrize = 1500, LeadWithPOPrize = 3000 },
                new RewardPrize { RewardClassId = 2, RewardCriteriaId = 3, LeadOnlyPrize = 2000, LeadWithPOPrize = 4000 },
                new RewardPrize { RewardClassId = 2, RewardCriteriaId = 4, LeadOnlyPrize = 2500, LeadWithPOPrize = 5000 },
                new RewardPrize { RewardClassId = 3, RewardCriteriaId = 1, LeadOnlyPrize = 500, LeadWithPOPrize = 1000 },
                new RewardPrize { RewardClassId = 3, RewardCriteriaId = 2, LeadOnlyPrize = 1000, LeadWithPOPrize = 2000 },
                new RewardPrize { RewardClassId = 3, RewardCriteriaId = 3, LeadOnlyPrize = 1500, LeadWithPOPrize = 3000 },
                new RewardPrize { RewardClassId = 3, RewardCriteriaId = 4, LeadOnlyPrize = 2000, LeadWithPOPrize = 4000 });
        }
    }
}
