using Lab05.Application.DTOs;
using Lab05.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Lab05.Api.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EvaluacionesController : ControllerBase
{
    private readonly IEvaluacionService _service;
    public EvaluacionesController(IEvaluacionService service) => _service = service;
    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _service.GetAllAsync());
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id) => Ok(await _service.GetByIdAsync(id));
    [HttpPost]
    public async Task<IActionResult> Post(EvaluacioneRequest request)
    {
        var entity = request.ToEntity();
        await _service.CreateAsync(entity);
        return CreatedAtAction(nameof(Get), new { id = entity.IdEvaluacion }, entity);
    }
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, EvaluacioneRequest request)
    {
        var entity = request.ToEntity();
        entity.IdEvaluacion = id;
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
