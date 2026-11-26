using System.ComponentModel.DataAnnotations;

namespace GestionProyectos.Api.Dtos;

public class LoginRequest
{
    [Required(ErrorMessage = "El correo es obligatorio.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    public string Contrasena { get; set; } = null!;
}

public class LoginResponse
{
    public string Token { get; set; } = null!;

    public DateTime Expira { get; set; }
}

public class MeResponse
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Rol { get; set; } = null!;
}
