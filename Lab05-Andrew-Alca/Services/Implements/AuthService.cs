using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Lab05_Andrew_Alca.DTOs;
using Lab05_Andrew_Alca.Models;
using Lab05_Andrew_Alca.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Lab05_Andrew_Alca.Services.Implements;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    private readonly PasswordHasher<Users> _passwordHasher = new();

    public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<Users> RegisterAsync(RegisterModel model)
    {
        var usuarios = await _unitOfWork.Repository<Users>().GetAllAsync();
        if (usuarios.Any(u => u.Username == model.Username))
        {
            throw new InvalidOperationException("El nombre de usuario ya existe.");
        }

        var usuario = new Users
        {
            Username = model.Username,
            Rol = model.Rol
        };
        usuario.PasswordHash = _passwordHasher.HashPassword(usuario, model.Password);

        await _unitOfWork.Repository<Users>().AddAsync(usuario);
        await _unitOfWork.Complete();
        return usuario;
    }

    public async Task<LoginResponse> LoginAsync(LoginModel model)
    {
        var usuarios = await _unitOfWork.Repository<Users>().GetAllAsync();
        var usuario = usuarios.FirstOrDefault(u => u.Username == model.Username) ??
                      throw new UnauthorizedAccessException("Usuario o contraseña incorrectos.");

        var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.PasswordHash, model.Password);
        if (resultado == PasswordVerificationResult.Failed)
            throw new UnauthorizedAccessException("Usuario o contraseña incorrectos.");
        
        var token = GenerarToken(usuario);
        var response = new LoginResponse()
        {
            Success = true,
            Token = token
        };
        return (response);
    }

    private string GenerarToken(Users usuario)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, usuario.Username),
            new Claim(ClaimTypes.Role, usuario.Rol)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
