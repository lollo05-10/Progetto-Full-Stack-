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
        var imageEntity = new Image
        {
            ReportId = reportId,
            Path = filePathFull
        };
        await _context.Image.AddAsync(imageEntity);
        await _context.SaveChangesAsync();
        using (Stream fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }
        return filePathFull;
    }
}
