using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntitiesConfiguration
{
    public class LeadNeedConfiguration : IEntityTypeConfiguration<LeadNeed>
    {
        public void Configure(EntityTypeBuilder<LeadNeed> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasOne(p => p.NeededDevice)
                .WithMany()
                .HasForeignKey(p => p.NeededDeviceId);
        }
    }
}
