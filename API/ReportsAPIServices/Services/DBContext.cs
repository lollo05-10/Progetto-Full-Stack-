using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ReportsAPIServices.Entities;

namespace ReportsAPIServices.Services;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
    public DbSet<Category> Category { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<Image> Image { get; set; }
    public DbSet<Report> Report { get; set; }
    public DbSet<ReportCategory> ReportCategories { get; set; }
    public DbSet<Permission> Permission { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("animal_reports");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
