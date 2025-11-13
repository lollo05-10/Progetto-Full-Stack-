using Microsoft.AspNetCore.Mvc;
using ReportsAPIServices.Models.DTOs;
using ReportsAPIServices.Models.DTOs.UpdateDTO;
using ReportsAPIServices.Services.Services_Interfaces;

namespace ReportsAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoryController : ControllerBase
{

    private readonly ICategoryService _serviceCategory;
    private readonly ILogger _logger;
    public CategoryController(ILogger<CategoryController> logger, ICategoryService serviceCategory)
    {
        _serviceCategory = serviceCategory;
        _logger = logger;
    }


    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _serviceCategory.GetAllCategoriesAsync();

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var result = await _serviceCategory.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] CategoryDTO addEntity)
    {
        var result = await _serviceCategory.AddAsync(addEntity);
        return Ok(result);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateCategoryDTO updateEntity, [FromRoute] int id)
    {
        await _serviceCategory.UpdateAsync(updateEntity, id);
        return Ok();
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteById([FromRoute] int id)
    {
        await _serviceCategory.DeleteByIdAsync(id);
        return Ok();
    }
    [HttpGet("name/{id}")]
    public async Task<IActionResult> GetCategoryName([FromRoute] int id)
    {
        var result = await _serviceCategory.GetNameAsync(id);
        if (result == "-1")
            return NotFound($"Categoria con ID {id} non trovata!");
        return Ok(result);
    }
    [HttpGet("description/{id}")]
    public async Task<IActionResult> GetCategoryDescription([FromRoute] int id)
    {
        var result = await _serviceCategory.GetDescriptionAsync(id);
        if (result == "-1")
            return NotFound($"Categoria con ID {id} non trovata!");
        return Ok(result);
    }
}
