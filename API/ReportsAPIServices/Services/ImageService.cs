using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ReportsAPIServices.Entities;
using ReportsAPIServices.Models.DTOs;
using ReportsAPIServices.Models.View_Models;
using ReportsAPIServices.Services.Services_Interfaces;

namespace ReportsAPIServices.Services;

public class ImageService : IImageService
{
    private readonly DatabaseContext _context;
    private readonly IConfiguration _config;
    public ImageService(DatabaseContext context, IConfiguration config)
    {
        _config = config;
        _context = context;
    }

    public async Task<int> DeleteImage(int id)
    {
        var img = await _context.Image.FindAsync(id);
        if (img == null)
            return -1;
        try
        {
            if (File.Exists(img.Path))
            {
                File.Delete(img.Path);
            }
            _context.Image.Remove(img);
            await _context.SaveChangesAsync();
            return img.Id;
        }
        catch
        {
            return -2;
        }
    }

    public async Task<List<ImageViewModel>> GetAllReportImages(int reportId)
    {
        var img_list = await _context.Image.ToListAsync();
        var imgViewModel = img_list
            .Where(x => x.ReportId == reportId)
            .Select(i => new ImageViewModel
            {
                Id = i.Id,
                Path = i.Path,
                ReportId = i.ReportId,
                Report = i.Report,
            })
            .ToList();
        return imgViewModel;
    }


    public async Task<string> UploadFileImages(IFormFile file, int reportId)
    {
        if (file.Length < 1)
        {
            return await Task.FromResult("-1");
        }

        bool exist = await _context.Report.Where(x => x.Id == reportId).AnyAsync();
        if (exist == false)
        {
            return await Task.FromResult("-2");
        }

        string? filePath = _config["FilePath"];
        if (string.IsNullOrWhiteSpace(filePath))
        {
            return await Task.FromResult("-3");
        }

        if (!Directory.Exists(filePath))
        {
            return await Task.FromResult("-4");
        }
        string filePathFull = Path.Combine(filePath, file.FileName);
        try
        {
            using (var fileStream = new FileStream(filePathFull, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            var imageEntity = new Image
            {
                ReportId = reportId,
                Path = filePathFull
            };

            await _context.Image.AddAsync(imageEntity);
            await _context.SaveChangesAsync();

            return filePathFull;
        }
        catch
        {
            return await Task.FromResult("-5"); 
        }
    }

    public async Task<string> UploadTemporaryImage(IFormFile file)
    {
        if (file == null || file.Length < 1)
        {
            return await Task.FromResult("-1");
        }

        string? filePath = _config["FilePath"];
        if (string.IsNullOrWhiteSpace(filePath))
        {
            return await Task.FromResult("-3");
        }

        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }

        // Genera un nome file univoco per evitare conflitti
        string fileName = $"{Guid.NewGuid()}_{file.FileName}";
        string filePathFull = Path.Combine(filePath, fileName);
        
        try
        {
            using (var fileStream = new FileStream(filePathFull, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // Restituisce il percorso relativo o assoluto (a seconda di come vuoi servire le immagini)
            // Per ora restituiamo il percorso relativo che inizierà con /images/
            string relativePath = $"/images/{fileName}";
            return relativePath;
        }
        catch (Exception ex)
        {
            return await Task.FromResult($"-5: {ex.Message}"); 
        }
    }

    public async Task<bool> LinkImagesToReport(List<string> imagePaths, int reportId)
    {
        if (imagePaths == null || !imagePaths.Any())
            return true;

        bool reportExists = await _context.Report.AnyAsync(r => r.Id == reportId);
        if (!reportExists)
            return false;

        try
        {
            string? baseFilePath = _config["FilePath"];
            if (string.IsNullOrWhiteSpace(baseFilePath))
                return false;

            foreach (var imagePath in imagePaths)
            {
                // Se il percorso è relativo (/images/...), costruiamo il percorso completo
                string fullPath = imagePath.StartsWith("/images/") 
                    ? Path.Combine(baseFilePath, imagePath.Replace("/images/", ""))
                    : imagePath;

                if (File.Exists(fullPath))
                {
                    var imageEntity = new Image
                    {
                        ReportId = reportId,
                        Path = imagePath // Salviamo il percorso relativo per servirlo via HTTP
                    };

                    await _context.Image.AddAsync(imageEntity);
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
