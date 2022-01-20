using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntitiesConfiguration
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.MessageAr).HasMaxLength(1000);
            builder.Property(p => p.MessageEn).HasMaxLength(1000);          

            builder.HasOne(p => p.User)
               .WithMany(p => p.Notifications)
               .HasForeignKey(p => p.UserId)
               .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.Lead)
               .WithMany()
               .HasForeignKey(p => p.LeadId)
               .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.LeadStatus)
               .WithMany()
               .HasForeignKey(p => p.LeadStatusId)
               .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
