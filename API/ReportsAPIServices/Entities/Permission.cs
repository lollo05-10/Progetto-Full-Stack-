using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportsAPIServices.Entities;

public class Permission
{
    public int Id { get; set; }
    public bool Can_Edit { get; set; } = false;
    public bool Can_View { get; set; } = true;
    public bool Admin_Su { get; set; } = false;
    public int UserID { get; set; }
    public virtual User User { get; set; }
}
