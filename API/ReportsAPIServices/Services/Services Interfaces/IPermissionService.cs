using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ReportsAPIServices.Models.DTOs;
using ReportsAPIServices.Models.DTOs.UpdateDTO;
using ReportsAPIServices.Models.View_Models;

namespace ReportsAPIServices.Services.Services_Interfaces;

public interface IPermissionService
{
    Task<PermissionViewModel> GetPermissionByUserId(int id);
    Task<int> UpdateAsync(UpdatePermissionDTO updateEntity);
}
