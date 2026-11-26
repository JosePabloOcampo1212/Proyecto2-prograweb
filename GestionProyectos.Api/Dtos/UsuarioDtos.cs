using System.ComponentModel.DataAnnotations;

namespace GestionProyectos.Api.Dtos;

public class UsuarioDto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int IdRol { get; set; }

    public string Rol { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public bool Activo { get; set; }
}

public class UsuarioGuardarDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
    [StringLength(150, ErrorMessage = "El correo no puede superar los 150 caracteres.")]
    public string Email { get; set; } = null!;

    public string? Contrasena { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un rol.")]
    public int IdRol { get; set; }

    public bool Activo { get; set; } = true;
}
