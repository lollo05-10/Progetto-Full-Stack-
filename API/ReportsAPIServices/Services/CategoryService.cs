using Microsoft.EntityFrameworkCore;
using ReportsAPIServices.Entities;
using ReportsAPIServices.Models.DTOs;
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

    public async Task<string> GetNameAsync()
    {
        var a = await _context.Category.SingleAsync(x => x.Id == 1);

        return a.Name;
    }

    public async Task<string> GetDescriptionAsync()
    {
        var a = await _context.Category.SingleAsync(x => x.Id == 2);
        return a.Description;
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

    public async Task DeleteByIdAsync(int id)
    {
        var category = await _context.Category.FindAsync(id);
        if (category != null)
        {
            _context.Category.Remove(category);
            await _context.SaveChangesAsync();

        }

    }

    public async Task UpdateAsync(CategoryDTO dto)
    {
        var category = new Category
        {
            Name = dto.Name,
            Description = dto.Description
        };
        _context.Update(category);
        await _context.SaveChangesAsync();
    }








}
