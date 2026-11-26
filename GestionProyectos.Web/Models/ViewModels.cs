using System.ComponentModel.DataAnnotations;

namespace GestionProyectos.Web.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
    [Display(Name = "Correo")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Contrasena { get; set; } = "";
}

public class UsuarioFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
    public string Nombre { get; set; } = "";

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
    [StringLength(150, ErrorMessage = "El correo no puede superar los 150 caracteres.")]
    [Display(Name = "Correo")]
    public string Email { get; set; } = "";

    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string? Contrasena { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un rol.")]
    [Display(Name = "Rol")]
    public int IdRol { get; set; }

    public bool Activo { get; set; } = true;

    public List<CatalogoItem> Roles { get; set; } = new();
}

