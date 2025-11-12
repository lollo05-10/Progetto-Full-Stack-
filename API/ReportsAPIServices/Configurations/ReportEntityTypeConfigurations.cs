using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportsAPIServices.Entities;

namespace ReportsAPIServices.Configurations;

public class ReportEntityTypeConfigurations : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(50);
        builder.HasIndex(x => x.Title)
            .IsUnique();
        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(75);
        builder.Property(x => x.ReportDate)
            .IsRequired()
            .HasColumnType("timestamp with time zone");
        builder.Property(x => x.Longitude)
            .IsRequired();
        builder.Property(x => x.Latitude)
            .IsRequired();
        builder
            .HasMany(x => x.Report_Images)
            .WithOne(x => x.Report)
            .HasForeignKey(x => x.ReportId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
