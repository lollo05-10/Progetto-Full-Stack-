using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportsAPIServices.Entities;

namespace ReportsAPIServices.Configurations;

public class CategoryEntityTypeConfigurations : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(75);
        builder.HasIndex(x => x.Name)
            .IsUnique();
        builder.Property(x => x.Description)
            .HasMaxLength(100);
    }
}
