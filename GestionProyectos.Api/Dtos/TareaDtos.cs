using System.ComponentModel.DataAnnotations;

namespace GestionProyectos.Api.Dtos;

public class TareaDto
{
    public int Id { get; set; }

    public int IdProyecto { get; set; }

    public string Titulo { get; set; } = null!;

    public string? Descripcion { get; set; }

    public int IdAsignado { get; set; }

    public string Asignado { get; set; } = null!;

    public int IdEstado { get; set; }

    public string Estado { get; set; } = null!;

    public int IdPrioridad { get; set; }

    public string Prioridad { get; set; } = null!;

    public DateTime FechaCreacion { get; set; }

    public DateOnly FechaVencimiento { get; set; }
}

public class TareaGuardarDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Debe indicar el proyecto.")]
    public int IdProyecto { get; set; }

    [Required(ErrorMessage = "El título es obligatorio.")]
    [StringLength(150, ErrorMessage = "El título no puede superar los 150 caracteres.")]
    public string Titulo { get; set; } = null!;

    [StringLength(1000, ErrorMessage = "La descripción no puede superar los 1000 caracteres.")]
    public string? Descripcion { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar el usuario asignado.")]
    public int IdAsignado { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un estado.")]
    public int IdEstado { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una prioridad.")]
    public int IdPrioridad { get; set; }

    [Required(ErrorMessage = "La fecha de vencimiento es obligatoria.")]
    public DateOnly? FechaVencimiento { get; set; }
}
