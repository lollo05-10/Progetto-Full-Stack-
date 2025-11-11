using ReportsAPIServices.Models.DTOs;
using ReportsAPIServices.Models.View_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportsAPIServices.Services.Services_Interfaces;

public interface ICategoryService
{
    Task<string> GetNameAsync();

    Task<string> GetDescriptionAsync();

    Task<List<CategoryViewModel>> GetAllCategoriesAsync();

    Task<CategoryViewModel> GetByIdAsync(int id);

    Task<int> AddAsync(CategoryDTO addEntity);

    Task UpdateAsync(CategoryDTO updateEntity);

    Task DeleteByIdAsync(int id);



}