using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.EntitiesConfiguration
{
    public class RewardClassConfiguration : IEntityTypeConfiguration<RewardClass>
    {
        public void Configure(EntityTypeBuilder<RewardClass> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(p => p.NameAr).HasMaxLength(200);
            builder.Property(p => p.NameEn).HasMaxLength(200);

            builder.HasData(
                new RewardClass {  Id = 1, NameEn = "Platinum", NameAr = "بلاتينيوم"},
                new RewardClass { Id = 2, NameEn = "Gold", NameAr = "جولد" },
                new RewardClass { Id = 3, NameEn = "Silver", NameAr = "سيلفر" });
        }
    }
}
