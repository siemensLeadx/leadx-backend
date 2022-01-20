using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntitiesConfiguration
{
    public class RewardCriteriaConfiguration : IEntityTypeConfiguration<RewardCriteria>
    {
        public void Configure(EntityTypeBuilder<RewardCriteria> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(p => p.From).HasColumnType("decimal(10, 2)");
            builder.Property(p => p.To).HasColumnType("decimal(10, 2)");

            builder.HasData(
                new RewardCriteria { Id = 1, To = 1000000 },
                new RewardCriteria { Id = 2, From = 1000000, To = 3000000 },
                new RewardCriteria { Id = 3,From = 3000000, To = 6000000 },
                new RewardCriteria { Id = 4, From = 6000000 });
        }
    }
}
