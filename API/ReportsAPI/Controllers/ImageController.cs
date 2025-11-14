using Microsoft.AspNetCore.Mvc;
using ReportsAPIServices.Models.View_Models;
using ReportsAPIServices.Services.Services_Interfaces;

namespace ReportsAPI.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ImageController : ControllerBase
{
    private readonly IImageService _service;
    public ImageController(IImageService service)
    {
        _service = service;
    }
    [HttpPost("upload/report/{reportId}")]
    public async Task<IActionResult> UploadFile(IFormFile file, [FromRoute] int reportId)
    {
        string result = await  _service.UploadFileImages(file, reportId);
        if (result == "-1")
            return BadRequest("File vuoto");
        if (result == "-2")
            return BadRequest($"Report con id {reportId} non trovato!");
        if (result == "-3")
            return BadRequest("Path del file vuoto!");
        if (result == "-4")
            return BadRequest("La directory non esiste");
        return Created("Immagine aggiunta: ", result);
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadTemporaryImage(IFormFile file)
    {
        if (file == null)
            return BadRequest("File non fornito");

        string result = await _service.UploadTemporaryImage(file);
        if (result.StartsWith("-"))
        {
            if (result == "-1")
                return BadRequest("File vuoto");
            if (result == "-3")
                return BadRequest("Path del file non configurato!");
            return BadRequest($"Errore durante l'upload: {result}");
        }
        return Ok(new { path = result });
    }
    [HttpGet("report/{reportId}/images")]
    public async Task<IActionResult> GetAllReportImages([FromRoute] int reportId)
    {
        List<ImageViewModel>? result = await _service.GetAllReportImages(reportId);
        if (result == null || !result.Any())
            return NoContent();
        return Ok(result);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        int result = await _service.DeleteImage(id);
        if (result == -1)
            return BadRequest($"Immagine con ID {id} non trovata!");
        if (result == -2)
            return BadRequest("Immagine non trovata!");
        return NoContent();
    }
}
