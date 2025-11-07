using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportsAPIServices.Entities;

namespace ReportsAPIServices.Configurations;

public class ReportCategoryEntityTypeConfiguration : IEntityTypeConfiguration<ReportCategory>
{
    public void Configure(EntityTypeBuilder<ReportCategory> builder)
    {
        builder.HasKey(rc => new { rc.ReportId, rc.CategoryId });
        builder
            .HasOne(rc => rc.Report)
            .WithMany(r => r.ReportCategories)
            .HasForeignKey(rc => rc.ReportId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(rc => rc.Category)
            .WithMany(c => c.ReportCategories)
            .HasForeignKey(rc => rc.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
