using System.ComponentModel.DataAnnotations;

namespace GestionProyectos.Api.Dtos;

public class ProyectoDto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaFinEstimada { get; set; }

    public int IdEstado { get; set; }

    public string Estado { get; set; } = null!;

    public int IdResponsable { get; set; }

    public string Responsable { get; set; } = null!;

    public DateTime FechaCreacion { get; set; }
}

public class ProyectoGuardarDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(150, ErrorMessage = "El nombre no puede superar los 150 caracteres.")]
    public string Nombre { get; set; } = null!;

    [StringLength(1000, ErrorMessage = "La descripción no puede superar los 1000 caracteres.")]
    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
    public DateOnly? FechaInicio { get; set; }

    [Required(ErrorMessage = "La fecha estimada de fin es obligatoria.")]
    public DateOnly? FechaFinEstimada { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un estado.")]
    public int IdEstado { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un responsable.")]
    public int IdResponsable { get; set; }
}
