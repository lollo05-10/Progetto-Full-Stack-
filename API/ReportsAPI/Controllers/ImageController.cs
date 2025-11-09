using Microsoft.AspNetCore.Mvc;
using ReportsAPIServices.Services.Services_Interfaces;

namespace ReportsAPI.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ImageController : ControllerBase
{
    private readonly IImageService _imgServ;
    public ImageController(IImageService imgServ)
    {
        _imgServ = imgServ;
    }
    [HttpPost("upload/report/{reportId}")]
    public async Task<IActionResult> UploadFile(IFormFile file, [FromRoute] int reportId)
    {
        string result = await  _imgServ.UploadFileImages(file, reportId);
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
}
