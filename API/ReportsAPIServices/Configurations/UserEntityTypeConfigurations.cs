using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportsAPIServices.Entities;

namespace ReportsAPIServices.Configurations;

public class UserEntityTypeConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Username)
            .IsRequired()
            .HasMaxLength(10);
        builder.HasIndex(x => x.Username)
            .IsUnique();
        builder.Property(x => x.Gender)
            .IsRequired()
            .HasMaxLength(1);
        builder.Property(x => x.DOB)
            .HasColumnType("date");
        builder.Property(x => x.isAdmin)
            .HasColumnType("boolean")
            .IsRequired();
        builder
            .HasMany(x => x.User_Reports)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserID)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        builder.ToTable(t => t.HasCheckConstraint(
            "CK_User_Gender",
            "\"Gender\" IN ('M', 'F')"
            ));

    }
}
