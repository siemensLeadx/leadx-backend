using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntitiesConfiguration
{
    public class LeadStatusConfiguration : IEntityTypeConfiguration<LeadStatus>
    {
        public void Configure(EntityTypeBuilder<LeadStatus> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(p => p.NameAr).HasMaxLength(200);
            builder.Property(p => p.NameEn).HasMaxLength(200);
            builder.Property(p => p.BackgroundColor).HasMaxLength(30);
            builder.Property(p => p.TextColor).HasMaxLength(30);

            builder.HasData(
                new LeadStatus { Id = LeadStatuses.New, NameEn = "New", NameAr = "جديد", BackgroundColor = "#999999", TextColor = "#000000" },
                new LeadStatus { Id = LeadStatuses.Verified, NameEn = "Verified by DCE", NameAr = "تم التحقق", BackgroundColor = "#fdddcb", TextColor = "#cf4b00" },
                new LeadStatus { Id = LeadStatuses.Approved, NameEn = "Approved", NameAr = "تم الموافقة", BackgroundColor = "#c8e6e6", TextColor = "#006f6f" },
                new LeadStatus { Id = LeadStatuses.Rejected, NameEn = "Rejected", NameAr = "مرفوضة", BackgroundColor = "#f9bfc7", TextColor = "#d9001d" },
                new LeadStatus { Id = LeadStatuses.Promoted, NameEn = "Promoted", NameAr = "تم الترقية", BackgroundColor = "#ceeffb", TextColor = "#3abfed" },
                new LeadStatus { Id = LeadStatuses.Ordered, NameEn = "Order Booked", NameAr = "تم الطلب", BackgroundColor = "#bfe6cd", TextColor = "#009a38" });
        }
    }
}
