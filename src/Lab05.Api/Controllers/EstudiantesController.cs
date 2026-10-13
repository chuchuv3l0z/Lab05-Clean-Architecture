using Lab05.Application.DTOs;
using Lab05.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Lab05.Api.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EstudiantesController : ControllerBase
{
    private readonly IEstudianteService _service;
    public EstudiantesController(IEstudianteService service) => _service = service;
    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _service.GetAllAsync());
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id) => Ok(await _service.GetByIdAsync(id));
    [HttpPost]
    public async Task<IActionResult> Post(EstudianteRequest request)
    {
        var entity = request.ToEntity();
        await _service.CreateAsync(entity);
        return CreatedAtAction(nameof(Get), new { id = entity.IdEstudiante }, entity);
    }
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, EstudianteRequest request)
    {
        var entity = request.ToEntity();
        entity.IdEstudiante = id;
        await _service.UpdateAsync(entity);
        return NoContent();
    }
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
