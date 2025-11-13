using ReportsAPIServices.Models.DTOs;
using ReportsAPIServices.Models.Filters;
using ReportsAPIServices.Models.View_Models;
using ReportsAPIServices.Models.DTOs.UpdateDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoJSON.Net;
using GeoJSON.Net.Feature;

namespace ReportsAPIServices.Services.Services_Interfaces;

public interface IReportService
{


    Task<List<ReportViewModel>> GetAllReportsAsync();

    Task<List<ReportViewModel>> GetByFilter(ReportFilter filter);

    Task<ReportViewModel> GetByIdAsync(int id);

    Task<int> AddAsync(ReportDTO addEntity);

    Task UpdateAsync(UpdateReportDTO updateEntity);

    Task DeleteByIdAsync(int id);
    Task<FeatureCollection> GetReportsGeoJSON();
}
