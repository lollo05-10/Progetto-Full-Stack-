using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReportsAPIServices.Models.DTOs;
using ReportsAPIServices.Models.DTOs.UpdateDTO;
using ReportsAPIServices.Models.Filters;
using ReportsAPIServices.Models.View_Models;

namespace ReportsAPIServices.Services.Services_Interfaces;

public interface IUserService
{
    Task<int> AddUser(UserDTO addEntity);
    Task<UserViewModel> GetById(int id);
    Task<List<UserViewModel>> GetByFilter(UserFilter filter);
    Task<int> UpdatePatch(UpdateUserDTO updateEntity, int id);
    Task<int> Delete(int id);
    Task<List<UserViewModel>> GetAllUsers();
    Task<int> GetIdByUsername(string Username);
}
