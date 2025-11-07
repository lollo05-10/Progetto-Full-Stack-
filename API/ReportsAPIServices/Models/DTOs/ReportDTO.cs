using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReportsAPIServices.Entities;

namespace ReportsAPIServices.Models.DTOs;

public class ReportDTO
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int UserId { get; set; }
    public virtual User User { get; set; }
    public DateTime ReportDate { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public List<ReportCategory>? Report_Categories { get; set; } 
    public List<ImageDTO>? Images { get; set; }
}
