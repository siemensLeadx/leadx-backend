using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntitiesConfiguration
{
    public class NeededDeviceConfiguration : IEntityTypeConfiguration<NeededDevice>
    {
        public void Configure(EntityTypeBuilder<NeededDevice> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(p => p.NameAr).HasMaxLength(500);
            builder.Property(p => p.NameEn).HasMaxLength(500);

            builder.HasData(
                new NeededDevice { Id = 1, NameEn = "Advanced Therapy (AT)", NameAr = "أجهزة العلاج المتقدم" },
                new NeededDevice { Id = 2, NameEn = "Magnetic Resonance (MR)", NameAr = "أجهزة تصوير الرنين المغناطيسي" },
                new NeededDevice { Id = 3, NameEn = "Molecular Imaging (MI)", NameAr = "أجهزة التصوير الجزيئي" },
                new NeededDevice { Id = 4, NameEn = "Computed Tomography (CT)", NameAr = "أجهزة التصوير المقطعي" },
                new NeededDevice { Id = 5, NameEn = "X-Ray (XP)", NameAr = "أجهزة الأشعة السينية" },
                new NeededDevice { Id = 6, NameEn = "Ultrasound (US)", NameAr = "أجهزة الموجات فوق الصوتية" },
                new NeededDevice { Id = 7, NameEn = "Digital Health (DH)", NameAr = "الأنظمة الصحية الرقمية" },
                new NeededDevice { Id = 8, NameEn = "SY", NameAr = "أنظمة معالجة الصور الرقمية" });
        }
    }
}
