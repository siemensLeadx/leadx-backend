using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntitiesConfiguration
{
    public class CustomerStatusConfiguration : IEntityTypeConfiguration<CustomerStatus>
    {
        public void Configure(EntityTypeBuilder<CustomerStatus> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(p => p.NameAr).HasMaxLength(200);
            builder.Property(p => p.NameEn).HasMaxLength(200);

            builder.HasData(
                new CustomerStatus { Id = CustomerStatuses.Siemens, NameEn = "Siemens", NameAr = "موقع سيمنس" },
                new CustomerStatus { Id = CustomerStatuses.Competitor, NameEn = "Competitor", NameAr = "موقع لمنافس" },
                new CustomerStatus { Id = CustomerStatuses.New, NameEn = "New customer", NameAr = "عميل جديد" });
        }
    }
}
