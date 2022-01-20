using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.EntitiesConfiguration
{
    public class BusinessOpportunityTypeConfiguration : IEntityTypeConfiguration<BusinessOpportunityType>
    {
        public void Configure(EntityTypeBuilder<BusinessOpportunityType> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(p => p.NameAr).HasMaxLength(200);
            builder.Property(p => p.NameEn).HasMaxLength(200);

            builder.HasData(
                new BusinessOpportunityType { Id = BusinessOpportunityTypes.New, NameEn = "New", NameAr = "جهاز جديد" },
                new BusinessOpportunityType { Id = BusinessOpportunityTypes.Replacement, NameEn = "Replacement", NameAr = "استبدال جهاز قديم" });
        }
    }
}
