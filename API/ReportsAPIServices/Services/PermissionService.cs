using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReportsAPIServices.Entities;
using ReportsAPIServices.Models.DTOs.UpdateDTO;
using ReportsAPIServices.Models.View_Models;
using ReportsAPIServices.Services.Services_Interfaces;

namespace ReportsAPIServices.Services;

public class PermissionService : IPermissionService
{
    private readonly DatabaseContext _context;
    public PermissionService(DatabaseContext context)
    {
        _context = context;
    }
    public async Task<PermissionViewModel> GetPermissionByUserId(int id)
    {
        Permission? entity = await _context.Permission.FirstOrDefaultAsync(x => x.UserID == id);
        PermissionViewModel entityViewModel = null;
        if (entity != null)
        {
            entityViewModel = new PermissionViewModel
            {
                Id = entity.Id,
                Username = entity.User.Username,
                Can_Edit = entity.Can_Edit,
                Can_View = entity.Can_View,
                Admin_Su = entity.User.isAdmin,
            };
        }
        return entityViewModel;
    }

    public async Task<int> UpdateAsync(UpdatePermissionDTO updateEntity)
    {
        Permission? entity = await _context.Permission.FirstOrDefaultAsync(x => x.UserID == updateEntity.UserID);
        if (entity == null)
            return -1;
        if (updateEntity.Can_Edit.HasValue)
            entity.Can_Edit = updateEntity.Can_Edit.Value;
        if (updateEntity.Can_View.HasValue)
            entity.Can_View = updateEntity.Can_View.Value;
        if (updateEntity.Admin_Su.HasValue)
            entity.Admin_Su = updateEntity.Admin_Su.Value;
        _context.Permission.Update(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }
}