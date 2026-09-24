namespace Lab05_Andrew_Alca.Models;

public partial class Users
{
    public int IdUsuario { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Rol { get; set; } = null!;
}
