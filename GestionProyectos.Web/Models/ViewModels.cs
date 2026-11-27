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

public class ProyectoFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(150, ErrorMessage = "El nombre no puede superar los 150 caracteres.")]
    public string Nombre { get; set; } = "";

    [StringLength(1000, ErrorMessage = "La descripción no puede superar los 1000 caracteres.")]
    [Display(Name = "Descripción")]
    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Display(Name = "Fecha de inicio")]
    public DateOnly? FechaInicio { get; set; }

    [Required(ErrorMessage = "La fecha estimada de fin es obligatoria.")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Display(Name = "Fecha estimada de fin")]
    public DateOnly? FechaFinEstimada { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un estado.")]
    [Display(Name = "Estado")]
    public int IdEstado { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un responsable.")]
    [Display(Name = "Responsable")]
    public int IdResponsable { get; set; }

    public List<CatalogoItem> Estados { get; set; } = new();

    public List<CatalogoItem> Responsables { get; set; } = new();
}
