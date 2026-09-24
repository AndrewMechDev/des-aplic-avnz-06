using Lab05_Andrew_Alca.DTOs;
using Lab05_Andrew_Alca.Extensions;
using Lab05_Andrew_Alca.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab05_Andrew_Alca.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        var resultado = await _service.RegisterAsync(model);
        return Ok(new { resultado.IdUsuario, resultado.Username, resultado.Rol });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModel model)
    {
        var resultado = await _service.LoginAsync(model);
        return Ok(resultado);
    }

    [Authorize(Policy = "Administrador")]
    [HttpGet("admin")]
    public IActionResult SoloAdmin()
    {
        return Ok(new { message = "Acceso permitido: rol Admin" });
    }

    [Authorize(Policy = "Usuario")]
    [HttpGet("perfil")]
    public IActionResult Perfil()
    {
        return Ok(new { message = $"Bienvenido {User.Identity!.Name}" });
    }
}
