using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ReportsAPIServices.Entities;
using ReportsAPIServices.Models.DTOs;
using ReportsAPIServices.Models.DTOs.UpdateDTO;
using ReportsAPIServices.Models.Filters;
using ReportsAPIServices.Models.View_Models;
using ReportsAPIServices.Services.Services_Interfaces;

namespace ReportsAPIServices.Services;

public class ReportService : IReportService
{

    private readonly DatabaseContext _context;
    private readonly IConfiguration _configuration;

    public ReportService(DatabaseContext context, IConfiguration configuration)
    {
        _configuration = configuration;
        _context = context;
    }


    public async Task<List<ReportViewModel>> GetAllReportsAsync()
    {

        var reports = await _context.Report
                 .Select(r => new ReportViewModel
                 {
                     Id = r.Id,
                     Title = r.Title,
                     Description = r.Description,
                     Longitude = r.Longitude,
                     Latitude = r.Latitude,
                     Username = r.User.Username,
                     Images = r.Report_Images.Select(img => new ImageViewModel
                     {
                         Id = img.Id,
                         ReportId = img.ReportId,
                         Path = img.Path
                     }).ToList(),
                     Categories = r.ReportCategories.Select(rc => new CategoryViewModel
                     {
                         Id = rc.Category.Id,
                         Name = rc.Category.Name
                     }).ToList()
                 })
          .ToListAsync();

        return reports;

    }



    public async Task<ReportViewModel> GetByIdAsync(int id)
    {
        var report = await _context.Report
            .AsNoTracking()
            .Where(r => r.Id == id)
            .Select(r => new ReportViewModel
            {
                Id = r.Id,
                UserId = r.UserID,
                ReportDate = r.ReportDate,
                Title = r.Title,
                Description = r.Description,
                Longitude = r.Longitude,
                Latitude = r.Latitude,
                Username = r.User.Username,
                Images = r.Report_Images.Select(img => new ImageViewModel
                {
                    Id = img.Id,
                    ReportId = img.ReportId,
                    Path = img.Path
                }).ToList(),
                Categories = r.ReportCategories.Select(rc => new CategoryViewModel
                {
                    Id = rc.Category.Id,
                    Name = rc.Category.Name,
                    Description = rc.Category.Description
                }).ToList()
            })
            .FirstOrDefaultAsync();

        if (report == null)
            throw new KeyNotFoundException($"Report con ID {id} non trovato.");

        return report;
    }



    public async Task<List<ReportViewModel>> GetByFilter(ReportFilter filter)
    {

        IQueryable<Report> query = _context.Report;

        // Filtro per titolo 
        if (!string.IsNullOrWhiteSpace(filter.Title))
        {
            string keyword = filter.Title.ToLower();
            query = query.Where(r => r.Title.ToLower().Contains(keyword));
        }

        // Filtro per username 
        if (!string.IsNullOrWhiteSpace(filter.Username))
        {
            string username = filter.Username.ToLower();
            query = query.Where(r => r.User.Username.ToLower().Contains(username));
        }

        // Filtro per data del report 
        if (filter.ReportDate.HasValue)
        {
            DateTime date = filter.ReportDate.Value.Date;
            query = query.Where(r => r.ReportDate == date);
        }

        // (Opzionale) filtro per utente se l’oggetto User è fornito
        if (filter.User != null)
        {
            query = query.Where(r => r.UserID == filter.User.Id);
        }


        var results = await query
            .OrderByDescending(r => r.ReportDate)
            .Select(r => new ReportViewModel
            {
                Id = r.Id,
                Title = r.Title,
                Description = r.Description,
                Username = r.User.Username,
                ReportDate = r.ReportDate,
                Longitude = r.Longitude,
                Latitude = r.Latitude,
                Images = r.Report_Images.Select(img => new ImageViewModel
                {
                    Id = img.Id,
                    Path = img.Path
                }).ToList(),
                Categories = r.ReportCategories.Select(rc => new CategoryViewModel
                {
                    Id = rc.Category.Id,
                    Name = rc.Category.Name,
                    Description = rc.Category.Description
                }).ToList()
            })
            .ToListAsync();

        return results;
    }

    public async Task<int> AddAsync(ReportDTO dto)
    {
        // Validazioni
        if (string.IsNullOrWhiteSpace(dto.Title))
            throw new ArgumentException("Il titolo è obbligatorio.");

        if (dto.Latitude is < -90 or > 90 || dto.Longitude is < -180 or > 180)
            throw new ArgumentException("Coordinate non valide.");

        // Verifica che l'utente esista
        var userExists = await _context.User.AnyAsync(u => u.Id == dto.UserId);
        if (!userExists)
            throw new ArgumentException($"Utente con ID {dto.UserId} non trovato.");

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {


            var report = new Report
            {
                UserID = dto.UserId,
                ReportDate = dto.ReportDate ?? DateTime.UtcNow,
                Title = dto.Title,
                Description = dto.Description,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                ReportCategories = new List<ReportCategory>(),
                Report_Images = new List<Image>()
            };

            // Collego le categorie se presenti
            if (dto.CategoryNames != null && dto.CategoryNames.Any())
            {
                foreach (var catName in dto.CategoryNames)
                {
                    var category = await _context.Category
                        .FirstOrDefaultAsync(c => c.Name.ToLower() == catName.ToLower());
                    if (category != null)
                    {
                        report.ReportCategories.Add(new ReportCategory
                        {
                            CategoryId = category.Id,
                            Report = report
                        });
                    }
                }
            }

            // Aggiungo le immagini se presenti
            if (dto.Images != null && dto.Images.Any())
            {
                foreach (var imageDto in dto.Images)
                {
                    if (string.IsNullOrWhiteSpace(imageDto.Path))
                        continue; // ignora immagini vuote

                    report.Report_Images.Add(new Image
                    {
                        Path = imageDto.Path
                    });
                }
            }

            _context.Report.Add(report);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return report.Id;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }





    public async Task UpdateAsync(UpdateReportDTO dto)
    {
        var report = await _context.Report.FindAsync(dto.Id);
        if (report == null)
            throw new KeyNotFoundException($"Report con ID {dto.Id} non trovato.");

        if (!string.IsNullOrWhiteSpace(dto.Title))
            report.Title = dto.Title;

        if (!string.IsNullOrWhiteSpace(dto.Description))
            report.Description = dto.Description;

        if (dto.Latitude != null)
        {
            if (dto.Latitude is < -90 or > 90)
                throw new ArgumentException("Latitudine non valida. Deve essere tra -90 e 90.");
            report.Latitude = (double)dto.Latitude;
        }

        if (dto.Longitude != null)
        {
            if (dto.Longitude is < -180 or > 180)
                throw new ArgumentException("Longitudine non valida. Deve essere tra -180 e 180.");
            report.Longitude = (double)dto.Longitude;
        }

        _context.Report.Update(report);
        await _context.SaveChangesAsync();
    }

    //  Elimina un report
    public async Task DeleteByIdAsync(int id)
    {
        var report = await _context.Report
            .Include(r => r.Report_Images)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (report == null)
            throw new KeyNotFoundException($"Report con ID {id} non trovato.");

        // rimuove eventuali immagini collegate (se delete restrict)
        if (report.Report_Images != null && report.Report_Images.Any())
            _context.Image.RemoveRange(report.Report_Images);

        _context.Report.Remove(report);
        await _context.SaveChangesAsync();
    }

    public async Task<FeatureCollection> GetReportsGeoJSON()
    {
        var reports = await _context.Report
            .Include(r => r.User)
            .Include(r => r.Report_Images)
            .Include(r => r.ReportCategories)
            .ThenInclude(r => r.Category)
            .ToListAsync();
        var features = new List<Feature>();
        foreach (var report in reports)
        {
            var point = new Point(new Position(report.Latitude, report.Longitude));
            var report_props = new Dictionary<string, object>
            {
                {"Id", report.Id },
                { "Title", report.Title },
                { "Description", report.Description },
                { "ReportDate", report.ReportDate },
                { "Username", report.User.Username },
                { "Images", report.Report_Images.Select(img => img.Path).ToList() },
                { "Categories" , report.ReportCategories.Select( rc => rc.Category.Name ).ToList() }
            };
            var feature = new Feature(point, report_props);
            features.Add(feature);
        }
        return new FeatureCollection(features);
    }
}
