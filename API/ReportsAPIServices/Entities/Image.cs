using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportsAPIServices.Entities;

public class Image
{
    public int Id { get; set; }
    public string Path { get; set; }
    public int ReportId { get; set; }
    public virtual Report Report { get; set; }
}
