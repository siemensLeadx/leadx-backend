using Application.Authorization;
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
    public class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            builder.ToTable(name: "Roles");

            builder.Property(m => m.Id).HasMaxLength(85);
            builder.Property(m => m.NormalizedName).HasMaxLength(85);
            builder.Property(m => m.Name).HasMaxLength(85);
            builder.Property(m => m.NameAr).HasMaxLength(85);
            builder.Property(m => m.Alias).HasMaxLength(85);

            builder.HasMany(e => e.UserRoles)
                .WithOne(e => e.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired(false);

            builder.HasMany(e => e.RoleClaims)
                .WithOne(e => e.Role)
                .HasForeignKey(rc => rc.RoleId)
                .IsRequired(false);

            SeedData(builder);
        }

        private static void SeedData(EntityTypeBuilder<AppRole> builder)
        {
            builder.HasData(
                new AppRole { 
                    Id = Guid.Parse("46362c4c-e900-475e-bf20-f15ee1f7bfc7"),
                    Name = "Admin", 
                    NormalizedName = "ADMIN",
                    Alias = DefaultRoles.ADMIN.ToString(),
                    NameAr = "مدير" ,
                    ConcurrencyStamp = "7eb425e8-5fbf-4b25-9895-57e5bfc434dc"
                },
                new AppRole
                {
                    Id = Guid.Parse("c2f8bc0a-e683-4333-bd45-f243ea0668ed"),
                    Name = "View Only",
                    NormalizedName = "VIEW ONLY",
                    Alias = DefaultRoles.VIEW_ONLY.ToString(),
                    NameAr = "اطلاع فقط",
                    ConcurrencyStamp = "ca502371-b205-41b7-ab64-d62697c361fc"
                },
                new AppRole
                {
                    Id = Guid.Parse("d92eef1a-1941-4266-92df-72d890a5b53a"),
                    Name = "User",
                    NormalizedName = "USER",
                    Alias = DefaultRoles.USER.ToString(),
                    NameAr = "مستخدم",
                    ConcurrencyStamp = "ec2c7cd5-6724-4dbd-96f8-f12a473a3fee"
                },
                new AppRole
                {
                    Id = Guid.Parse("bf2d7d78-62b9-4e07-b530-05e855b2144d"),
                    Name = "Blocked",
                    NormalizedName = "BLOCKED",
                    Alias = DefaultRoles.BLOCKED.ToString(),
                    NameAr = "محظور",
                    ConcurrencyStamp = "ec2c7cd5-6724-4dbd-96f8-f12a473a3fee"
                });
        }
    }
}
