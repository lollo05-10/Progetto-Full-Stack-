using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReportsAPIServices.Entities;

namespace ReportsAPIServices.Models.View_Models;

public class UserViewModel
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Gender { get; set; }
    public DateOnly DOB { get; set; }
    public virtual ICollection<Report> Reports { get; set; }
}
