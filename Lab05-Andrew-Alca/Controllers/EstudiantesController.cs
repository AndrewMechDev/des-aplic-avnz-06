using Lab05_Andrew_Alca.Extensions;
using Lab05_Andrew_Alca.Models;
using Lab05_Andrew_Alca.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab05_Andrew_Alca.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EstudiantesController : ControllerBase
{
    private readonly IEstudiantesService _service;

    public EstudiantesController(IEstudiantesService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var items = await _service.GetAllAsync();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var item = await _service.GetByIdAsync(id);
        return item.ToGetResult();
    }

    [HttpPost]
    public async Task<IActionResult> Post(Estudiantes entity)
    {
        var creado = await _service.CreateAsync(entity);
        return CreatedAtAction(nameof(Get), new { id = creado.IdEstudiante }, creado);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Estudiantes entity)
    {
        var updated = await _service.UpdateAsync(id, entity);
        return updated.ToUpdateResult();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted.ToDeleteResult();
    }
}
