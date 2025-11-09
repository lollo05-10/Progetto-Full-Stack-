using Microsoft.AspNetCore.Mvc;
using ReportsAPIServices.Models.DTOs;
using ReportsAPIServices.Models.DTOs.UpdateDTO;
using ReportsAPIServices.Models.Filters;
using ReportsAPIServices.Models.View_Models;
using ReportsAPIServices.Services.Services_Interfaces;

namespace ReportsAPI.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;
    public UserController(IUserService service)
    {
        _service = service;
    }
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] UserDTO addEntity)
    {
        int result = await _service.AddUser(addEntity);
        if (result == -1)
            return BadRequest("ERRORE: Username già esistente!");
        if (result == -2)
            return BadRequest("ERRORE: il parametro Username è nullo, vuoto o troppo lungo!");
        if (result == -3)
            return BadRequest("ERRORE: il parametro Gender deve essere o M o F!");
        if (result == -4)
            return BadRequest("ERRORE: il parametro DOB inserito è una data futura!");
        return Created("", result);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        UserViewModel? result = await _service.GetById(id);
        if (result == null)
            return NotFound($"ATTENZIONE! User con id {id} non trovato!");
        return Ok(result);
    }
    [HttpGet]
    public async Task<IActionResult> GetByFilter([FromQuery] UserFilter filter)
    {
        List<UserViewModel>? result = await _service.GetByFilter(filter);
        if (result == null || !result.Any())
            return NoContent();
        return Ok(result);
    }
    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdatePatch([FromRoute] int id, [FromBody] UpdateUserDTO updateEntity)
    {
        int result = await _service.UpdatePatch(updateEntity, id);
        if (result == -1)
            return BadRequest($"ERRORE: User con id {id} da aggiornare richiesto non esistente!");
        if (result == -2)
            return BadRequest($"ERRORE: Username inserito nullo o troppo lungo");
        if (result == -3)
            return BadRequest("ERRORE: il parametro Gender deve essere o M o F!");
        if (result == -4)
            return BadRequest("ERRORE: il parametro DOB inserito è una data futura!");
        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        int result = await _service.Delete(id);
        if (result == -1)
            return BadRequest($"User con ID {id} non trovato!");
        return NoContent();
    }
}
