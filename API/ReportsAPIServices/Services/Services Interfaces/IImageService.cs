using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ReportsAPIServices.Entities;
using ReportsAPIServices.Models.DTOs;
using ReportsAPIServices.Models.View_Models;

namespace ReportsAPIServices.Services.Services_Interfaces;

public interface IImageService
{
    Task<string> UploadFileImages(IFormFile file, int reportId);
    Task<string> UploadTemporaryImage(IFormFile file);
    Task<List<ImageViewModel>> GetAllReportImages(int reportId);
    Task<int> DeleteImage(int id);
    Task<bool> LinkImagesToReport(List<string> imagePaths, int reportId);
}
