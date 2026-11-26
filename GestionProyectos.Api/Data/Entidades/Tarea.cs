namespace GestionProyectos.Api.Data.Entidades;

public class Tarea
{
    public int Id { get; set; }

    public int IdProyecto { get; set; }

    public string Titulo { get; set; } = null!;

    public string? Descripcion { get; set; }

    public int IdAsignado { get; set; }

    public int IdEstado { get; set; }

    public int IdPrioridad { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateOnly FechaVencimiento { get; set; }

    public Proyecto Proyecto { get; set; } = null!;

    public Usuario Asignado { get; set; } = null!;

    public Estado Estado { get; set; } = null!;

    public Prioridad Prioridad { get; set; } = null!;
}
