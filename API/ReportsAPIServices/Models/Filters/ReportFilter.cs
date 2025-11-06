using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReportsAPIServices.Entities;

namespace ReportsAPIServices.Models.Filters;

public class ReportFilter
{
    public string? Title { get; set; }
    public virtual User User { get; set; }
    public string? Username { get; set; }
    public DateTime? ReportDate { get; set; }
}
