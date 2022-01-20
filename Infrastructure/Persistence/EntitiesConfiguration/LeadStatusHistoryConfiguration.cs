using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntitiesConfiguration
{
    public class LeadStatusHistoryConfiguration : IEntityTypeConfiguration<LeadStatusHistory>
    {
        public void Configure(EntityTypeBuilder<LeadStatusHistory> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Notes).HasMaxLength(2000);

            builder.HasOne(p => p.Status)
                .WithMany()
                .HasForeignKey(p => p.StatusId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.Lead)
                .WithMany(p => p.StatusHistory)
                .HasForeignKey(p => p.LeadId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.CreatedBy)
                .WithMany()
                .HasForeignKey(p => p.CreatedById);
        }
    }
}
