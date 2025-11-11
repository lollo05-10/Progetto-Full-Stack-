using Microsoft.AspNetCore.Mvc;
using ReportsAPIServices.Models.DTOs;
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
    public async Task<IActionResult> Update([FromBody] CategoryDTO updateEntity)
    {
        await _serviceCategory.UpdateAsync(updateEntity);
        return Ok();
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteById([FromRoute] int id)
    {
        await _serviceCategory.DeleteByIdAsync(id);
        return Ok();
    }













}
