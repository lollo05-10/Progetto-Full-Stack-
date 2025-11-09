
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ReportsAPIServices.Services;
using ReportsAPIServices.Services.Services_Interfaces;
namespace ReportsAPI;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
        builder.Services.AddScoped<IImageService, ImageService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IReportService, ReportService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
