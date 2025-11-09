using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ReportsAPIServices.Entities;
using ReportsAPIServices.Models.DTOs;

namespace ReportsAPIServices.Services.Services_Interfaces;

public interface IImageService
{
    Task<string> UploadFileImages(IFormFile file, int reportId);
}
