using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportsAPIServices.Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Gender { get; set; }
    public DateOnly DOB { get; set; }
    public bool isAdmin { get; set; }
    public virtual ICollection<Report> User_Reports { get; set; }
    public virtual Permission Permission { get; set; }
}
