using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntitiesConfiguration
{
    public class LeadConfiguration : IEntityTypeConfiguration<Lead>
    {
        public void Configure(EntityTypeBuilder<Lead> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.LeadName).HasMaxLength(500);
            builder.Property(p => p.HospitalName).HasMaxLength(500);
            builder.Property(p => p.City).HasMaxLength(500);
            builder.Property(p => p.Comment).HasMaxLength(2000);
            builder.Property(p => p.ContactPerson).HasMaxLength(500);
            builder.Property(p => p.PromotedPrize).HasColumnType("decimal(8, 2)");
            builder.Property(p => p.OrderedPrize).HasColumnType("decimal(8, 2)");
            builder.Ignore(p => p.NotifyUser);

            builder.HasOne(p => p.BusinessOpportunityType)
               .WithMany(p => p.Leads)
               .HasForeignKey(p => p.BusinessOpportunityTypeId);

            builder.HasOne(p => p.CustomerStatus)
               .WithMany(p => p.Leads)
               .HasForeignKey(p => p.CustomerStatusId);

            builder.HasOne(p => p.CurrentLeadStatus)
                .WithMany(p => p.Leads)
                .HasForeignKey(p => p.CurrentLeadStatusId);

            builder.HasOne(p => p.User)
                .WithMany(p => p.Leads)
                .HasForeignKey(p => p.UserId);

            builder.HasMany(p => p.LeadNeeds)
                .WithOne(p => p.Lead)
                .HasForeignKey(p => p.LeadId);

            builder.HasMany(p => p.StatusHistory)
                .WithOne(p => p.Lead)
                .HasForeignKey(p => p.LeadId);

            builder.HasOne(p => p.RewardClass)
                .WithMany()
                .HasForeignKey(p => p.RewardClassId);

            builder.HasOne(p => p.RewardCriteria)
               .WithMany()
               .HasForeignKey(p => p.RewardCriteriaId);

            builder.HasOne(p => p.RegionArea)
              .WithMany(p => p.Leads)
              .HasForeignKey(p => p.RegionId);

            builder.HasOne(p => p.Sector)
              .WithMany(p => p.Leads)
              .HasForeignKey(p => p.SectorId);
        }
    }
}
