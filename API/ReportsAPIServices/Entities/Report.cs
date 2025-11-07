using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportsAPIServices.Entities;

public class Report
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime ReportDate { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public int UserID { get; set; }
    public virtual User User { get; set; }
    public virtual ICollection<Image> Report_Images { get; set; }
    public virtual ICollection<ReportCategory> ReportCategories { get; set; }

}
