using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportsAPIServices.Entities;

namespace ReportsAPIServices.Configurations;

public class PermissionEntityTypeConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Can_Edit)
            .HasColumnType("boolean")
            .HasDefaultValue(false)
            .IsRequired();
        builder
            .Property(x => x.Can_View)
            .HasColumnType("boolean")
            .HasDefaultValue(true)
            .IsRequired();
        builder
            .Property(x => x.Admin_Su)
            .HasColumnType("boolean")
            .HasDefaultValue(false)
            .IsRequired();
        builder
            .HasOne(x => x.User)
            .WithOne(x => x.Permission)
            .HasForeignKey<Permission>(x => x.UserID)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
