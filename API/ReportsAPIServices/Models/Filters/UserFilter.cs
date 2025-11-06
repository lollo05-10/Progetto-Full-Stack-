using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportsAPIServices.Models.Filters;

public class UserFilter
{
    public string? Username { get; set; }
    public string? Gender { get; set; }
    public DateOnly? DOB { get; set; }
}
