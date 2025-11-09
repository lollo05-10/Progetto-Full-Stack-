using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReportsAPIServices.Entities;

namespace ReportsAPIServices.Models.DTOs;

public class UserDTO
{
    public string Username { get; set; }
    public string Gender { get; set; }
    public DateOnly DOB { get; set; }
    public bool isAdmin { get; set; }
}
