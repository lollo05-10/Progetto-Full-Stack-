using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReportsAPIServices.Entities;

namespace ReportsAPIServices.Models.View_Models;

public class PermissionViewModel
{
    public int Id { get; set; }
    public string Username { get; set; }
    public bool Can_Edit { get; set; }
    public bool Can_View { get; set; }
    public bool Admin_Su { get; set; }
}