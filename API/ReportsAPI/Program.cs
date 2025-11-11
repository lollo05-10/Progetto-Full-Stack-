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


        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAngular", policy =>
            {
                policy.WithOrigins(
                    "http://localhost:4200",
                    "https://localhost:4200"
                    )
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
        }); 


        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Connessione al database PostgreSQL
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
        app.UseCors("AllowAngular");
        app.UseAuthorization();
       
        app.MapControllers();

        app.Run();
    }
}
