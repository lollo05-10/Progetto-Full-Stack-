using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReportsAPIServices.Entities;

namespace ReportsAPIServices.Models.View_Models;

public class ReportViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

    public int? UserId { get; set; }
    public string Username { get; set; }
    public DateTime ReportDate { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public virtual ICollection<ImageViewModel> Images { get; set; }
    public virtual ICollection<CategoryViewModel> Categories { get; set; }
}
