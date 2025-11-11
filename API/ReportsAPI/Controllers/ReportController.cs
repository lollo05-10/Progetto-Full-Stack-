

using Microsoft.AspNetCore.Mvc;
using ReportsAPIServices.Models.DTOs;
using ReportsAPIServices.Models.DTOs.UpdateDTO;
using ReportsAPIServices.Models.Filters;
using ReportsAPIServices.Services.Services_Interfaces;

namespace ReportsAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    //  GET api/report
    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var reports = await _reportService.GetAllReportsAsync();
        return Ok(reports);
    }

    //  GET api/report/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var report = await _reportService.GetByIdAsync(id);
            return Ok(report);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    //  POST api/report/filter
    [HttpPost("filter")]
    public async Task<IActionResult> GetByFilter([FromBody] ReportFilter filter)
    {
        var reports = await _reportService.GetByFilter(filter);
        return Ok(reports);
    }

    //  POST api/report
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] ReportDTO dto)
    {
        try
        {
            var id = await _reportService.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    //  PUT api/report
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateReportDTO dto)
    {
        try
        {
            await _reportService.UpdateAsync(dto);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    //  DELETE api/report/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _reportService.DeleteByIdAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}

