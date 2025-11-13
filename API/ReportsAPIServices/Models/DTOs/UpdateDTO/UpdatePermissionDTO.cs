using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportsAPIServices.Models.DTOs.UpdateDTO;

public class UpdatePermissionDTO
{
    public int UserID { get; set; }
    public bool? Can_Edit { get; set; }
    public bool? Can_View { get; set; }
    public bool? Admin_Su { get; set; }
}
