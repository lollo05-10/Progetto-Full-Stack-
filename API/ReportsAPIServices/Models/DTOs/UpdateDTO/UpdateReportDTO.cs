using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReportsAPIServices.Entities;

namespace ReportsAPIServices.Models.DTOs.UpdateDTO;

public class UpdateReportDTO
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? ReportDate { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public virtual ICollection<Image>? Images { get; set; }
}
