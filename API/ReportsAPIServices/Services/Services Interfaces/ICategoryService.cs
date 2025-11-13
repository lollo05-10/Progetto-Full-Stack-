using ReportsAPIServices.Models.DTOs;
using ReportsAPIServices.Models.DTOs.UpdateDTO;
using ReportsAPIServices.Models.View_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportsAPIServices.Services.Services_Interfaces;

public interface ICategoryService
{

    Task<List<CategoryViewModel>> GetAllCategoriesAsync();

    Task<CategoryViewModel> GetByIdAsync(int id);

    Task<int> AddAsync(CategoryDTO addEntity);

    Task<int> UpdateAsync(UpdateCategoryDTO updateEntity, int id);

    Task<int> DeleteByIdAsync(int id);
    Task<string> GetNameAsync(int id);
    Task<string> GetDescriptionAsync(int id);
}