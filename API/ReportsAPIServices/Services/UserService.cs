using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReportsAPIServices.Entities;
using ReportsAPIServices.Models.DTOs;
using ReportsAPIServices.Models.DTOs.UpdateDTO;
using ReportsAPIServices.Models.Filters;
using ReportsAPIServices.Models.View_Models;
using ReportsAPIServices.Services.Services_Interfaces;

namespace ReportsAPIServices.Services;

public class UserService : IUserService
{
    private readonly DatabaseContext _context;
    public UserService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<int> AddUser(UserDTO addEntity)
    {
        bool exist = await _context.User
            .AsQueryable()
            .Where(x => x.Username == addEntity.Username)
            .AnyAsync();

        if (exist == true)
            return -1;
        var entityToAdd = new User
        {
            Username = addEntity.Username,
            Gender = addEntity.Gender,
            DOB = addEntity.DOB,
            isAdmin = addEntity.isAdmin,
        };
        if ((string.IsNullOrEmpty(addEntity.Username)) || (addEntity.Username.Length > 10))
            return -2;
        if (addEntity.Gender != "M" && addEntity.Gender != "F")
            return -3;
        if (addEntity.DOB > DateOnly.FromDateTime(DateTime.Now))
            return -4;
        await _context.User.AddAsync(entityToAdd);
        await _context.SaveChangesAsync();
        return entityToAdd.Id;
    }

    public async Task<int> Delete(int id)
    {
        User? entity = await _context.User.FindAsync(id);
        if (entity == null) 
            return -1;
        _context.Remove(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task<List<UserViewModel>> GetAllUsers()
    {
        var user_list = await _context.User
            .Include(u => u.User_Reports)
            .ToListAsync();
        var userViewModelList = user_list.Select(u => new UserViewModel
        {
            Id = u.Id,
            Username = u.Username,
            Gender = u.Gender,
            DOB = u.DOB,
            Reports = u.User_Reports
        }).ToList();
        return userViewModelList;
    }

    public async Task<List<UserViewModel>> GetByFilter(UserFilter filter)
    {
        IQueryable<User> query = _context.User.AsQueryable();
        if (filter.DOB != null)
            query = query.Where(x => x.DOB == filter.DOB);
        if (!string.IsNullOrWhiteSpace(filter.Username))
            query = query.Where(x => x.Username == filter.Username);
        if (!string.IsNullOrWhiteSpace(filter.Gender))
            query = query.Where(x => x.Gender == filter.Gender);
        List<User> list = await query.ToListAsync();
        List<UserViewModel> listViewModel = list.Select(x => new UserViewModel
        {
            Username = x.Username,
            DOB = x.DOB,
            Gender = x.Gender,
            Id = x.Id
        }).ToList();
        return listViewModel;
    }

    public async Task<UserViewModel> GetById(int id)
    {
        User? entity = await _context.User.FindAsync(id);
        UserViewModel entityViewModel = null;
        if (entity != null)
        {
            entityViewModel = new UserViewModel
            {
                Id = entity.Id,
                Username = entity.Username,
                Gender = entity.Gender,
                DOB = entity.DOB,
            };
        }
        return entityViewModel;
    }

    public async Task<int> GetIdByUsername(string Username)
    {
        User? entity = await _context.User.FirstOrDefaultAsync(u => u.Username == Username);
        if (entity == null)
            return -1;
        return entity.Id;
    }

    public async Task<int> UpdatePatch(UpdateUserDTO updateEntity, int id)
    {
        User? entity = await _context.User.FindAsync(id);
        if (entity == null)
            return -1;
        if (!string.IsNullOrEmpty(updateEntity.Username))
        {
            if (updateEntity.Username.Length > 10)
                return -2;
            entity.Username = updateEntity.Username;
        }

        if (!string.IsNullOrEmpty(updateEntity.Gender))
        {
            if (updateEntity.Gender != "M" && updateEntity.Gender != "F")
                return -3;
            entity.Gender = updateEntity.Gender;
        }

        if (updateEntity.DOB != null)
        {
            if (updateEntity.DOB > DateOnly.FromDateTime(DateTime.Now))
                return -4;
            entity.DOB = updateEntity.DOB.Value;
        }
        _context.User.Update(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }
}
