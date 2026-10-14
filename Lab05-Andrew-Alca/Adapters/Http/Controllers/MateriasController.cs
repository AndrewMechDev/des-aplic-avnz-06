using Lab05_Andrew_Alca.Adapters.Http.Extensions;
using Lab05_Andrew_Alca.Models;
using Lab05_Andrew_Alca.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab05_Andrew_Alca.Adapters.Http.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MateriasController : ControllerBase
{
    private readonly IMateriasService _service;

    public MateriasController(IMateriasService service)
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
    public async Task<IActionResult> Post(Materias entity)
    {
        var creado = await _service.CreateAsync(entity);
        return CreatedAtAction(nameof(Get), new { id = creado.IdMateria }, creado);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Materias entity)
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
