using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ReportsAPIServices.Entities;

public class ReportCategory
{
    public int ReportId { get; set; }
    public int CategoryId { get; set; }
    public virtual Report Report { get; set; }
    public virtual Category Category { get; set; }
}
