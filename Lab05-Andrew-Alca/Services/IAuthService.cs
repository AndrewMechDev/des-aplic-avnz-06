using Lab05_Andrew_Alca.DTOs;
using Lab05_Andrew_Alca.Models;

namespace Lab05_Andrew_Alca.Services;

public interface IAuthService
{
    Task<Users> RegisterAsync(RegisterModel model);
    Task<LoginResponse> LoginAsync(LoginModel model);
}
