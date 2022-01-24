using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntitiesConfiguration
{
    public class RegionConfiguration : IEntityTypeConfiguration<RegionArea>
    {
        public void Configure(EntityTypeBuilder<RegionArea> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(p => p.NameAr).HasMaxLength(200);
            builder.Property(p => p.NameEn).HasMaxLength(200);

            builder.HasData(
                new RegionArea { Id = Regions.Eastern, NameEn = "Eastern", NameAr = "الشرقية" },
                new RegionArea { Id = Regions.Western, NameEn = "Western", NameAr = "الغربية" },
                new RegionArea { Id = Regions.Southern, NameEn = "Southern", NameAr = "الجنوبية" },
                new RegionArea { Id = Regions.Northern, NameEn = "Northern", NameAr = "الشمالية" });
            }
    }
}
