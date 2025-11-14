using Microsoft.EntityFrameworkCore;
using ReportsAPIServices.Entities;
using ReportsAPIServices.Models.DTOs;
using ReportsAPIServices.Models.DTOs.UpdateDTO;
using ReportsAPIServices.Models.View_Models;
using ReportsAPIServices.Services.Services_Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportsAPIServices.Services;

public class CategoryService : ICategoryService
{
    private readonly DatabaseContext _context;



    public CategoryService(DatabaseContext context)
    {
        _context = context;
    }



    public async Task<List<CategoryViewModel>> GetAllCategoriesAsync()
    {

        var categories = await _context.Category
                 .Select(c => new CategoryViewModel
                 {
                     Name = c.Name,
                     Description = c.Description
                 })
                 .ToListAsync();
        return categories;

    }

    public async Task<CategoryViewModel> GetByIdAsync(int id)
    {
        var category = await _context.Category.SingleAsync(x => x.Id == id);

        var viewModel = new CategoryViewModel
        {

            Name = category.Name,
            Description = category.Description
        };

        return viewModel;

    }

    public async Task<int> AddAsync(CategoryDTO dto)
    {

        var category = new Category
        {
            Name = dto.Name,
            Description = dto.Description
        };
        await _context.AddAsync(category);
        await _context.SaveChangesAsync();
        return category.Id;
    }

    public async Task<int> DeleteByIdAsync(int id)
    {
        var category = await _context.Category.FindAsync(id);
        if (category == null)
            return -1;
        _context.Category.Remove(category);
        await _context.SaveChangesAsync();
        return category.Id;
    }

    public async Task<int> UpdateAsync(UpdateCategoryDTO updateEntity, int id)
    {
        Category? entity = await _context.Category.FindAsync(id);
        if (entity == null)
            return -1;
        if (!string.IsNullOrEmpty(updateEntity.Name))
        {
            entity.Name = updateEntity.Name;
        }
        if (!string.IsNullOrEmpty(updateEntity.Description))
        {
            entity.Description = updateEntity.Description;
        }
        _context.Category.Update(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task<string> GetNameAsync(int id)
    {
        var entity = await _context.Category.FindAsync(id);
        if (entity == null)
            return "-1";
        return entity.Name;
    }

    public async Task<string> GetDescriptionAsync(int id)
    {
        var entity = await _context.Category.FindAsync(id);
        if (entity == null)
            return "-1";
        return entity.Description;
    }
}
