using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReportsAPIServices.Entities;

namespace ReportsAPIServices.Models.DTOs.UpdateDTO;

public class UpdateReportDTO
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? ReportDate { get; set; }
    public double? Altitude { get; set; }
    public double? Latitude { get; set; }
    public virtual ICollection<Image>? Images { get; set; }
}
