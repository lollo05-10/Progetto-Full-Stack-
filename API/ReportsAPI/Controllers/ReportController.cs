using Microsoft.AspNetCore.Mvc;
using ReportsAPIServices.Models.DTOs;
using ReportsAPIServices.Models.DTOs.UpdateDTO;
using ReportsAPIServices.Models.Filters;
using ReportsAPIServices.Services.Services_Interfaces;
using System.Text.Json;

namespace ReportsAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;
    private readonly ILogger<ReportController> _logger;

    public ReportController(IReportService reportService, ILogger<ReportController> logger)
    {
        _reportService = reportService;
        _logger = logger;
    }

    //  GET api/report
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var reports = await _reportService.GetAllReportsAsync();
            _logger.LogInformation($"GetAll: Restituiti {reports.Count()} report");
            return Ok(reports);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore in GetAll");
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    //  GET api/report/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var report = await _reportService.GetByIdAsync(id);
            _logger.LogInformation($"GetById: Report {id} trovato");
            return Ok(report);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning($"GetById: Report {id} non trovato");
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Errore in GetById per id {id}");
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    //  POST api/report/filter
    [HttpPost("filter")]
    public async Task<IActionResult> GetByFilter([FromBody] ReportFilter filter)
    {
        try
        {
            _logger.LogInformation($"GetByFilter: Filtro ricevuto: {JsonSerializer.Serialize(filter)}");
            var reports = await _reportService.GetByFilter(filter);
            _logger.LogInformation($"GetByFilter: Restituiti {reports.Count()} report");
            return Ok(reports);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore in GetByFilter");
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    //  POST api/report
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] ReportDTO dto)
    {
        try
        {
            _logger.LogInformation($"Add: Dati ricevuti: {JsonSerializer.Serialize(dto)}");

            if (dto == null)
            {
                _logger.LogWarning("Add: DTO è null");
                return BadRequest(new { message = "Dati non validi" });
            }

            var id = await _reportService.AddAsync(dto);
            _logger.LogInformation($"Add: Report creato con ID {id}");
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning($"Add: Argomento non valido - {ex.Message}");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore in Add: {Message}", ex.Message);
            var errorMessage = "Errore interno del server";
            if (ex.InnerException != null)
            {
                _logger.LogError(ex.InnerException, "Inner exception: {Message}", ex.InnerException.Message);
                errorMessage = ex.InnerException.Message;
            }
            return StatusCode(500, new { message = errorMessage, details = ex.Message });
        }
    }

    //  PUT api/report
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateReportDTO dto)
    {
        try
        {
            _logger.LogInformation($"Update: Dati ricevuti: {JsonSerializer.Serialize(dto)}");

            if (dto == null)
            {
                _logger.LogWarning("Update: DTO è null");
                return BadRequest(new { message = "Dati non validi" });
            }

            await _reportService.UpdateAsync(dto);
            _logger.LogInformation($"Update: Report aggiornato");
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning($"Update: Report non trovato - {ex.Message}");
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning($"Update: Argomento non valido - {ex.Message}");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore in Update");
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    //  DELETE api/report/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            _logger.LogInformation($"Delete: Richiesta eliminazione report {id}");
            await _reportService.DeleteByIdAsync(id);
            _logger.LogInformation($"Delete: Report {id} eliminato");
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning($"Delete: Report {id} non trovato");
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Errore in Delete per id {id}");
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    // GET api/report/geojson
    [HttpGet("geojson")]
    public async Task<IActionResult> GetGeoJSON()
    {
        try
        {
            _logger.LogInformation("GetGeoJSON: Richiesta GeoJSON");
            var geoJson = await _reportService.GetReportsGeoJSON();
            _logger.LogInformation("GetGeoJSON: GeoJSON generato con successo");
            return Ok(geoJson);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore in GetGeoJSON");
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }
}