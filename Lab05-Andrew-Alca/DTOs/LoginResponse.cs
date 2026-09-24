namespace Lab05_Andrew_Alca.DTOs;

public class LoginResponse
{
    public bool Success { get; set; }
    public string Token { get; set; } = null!;
}