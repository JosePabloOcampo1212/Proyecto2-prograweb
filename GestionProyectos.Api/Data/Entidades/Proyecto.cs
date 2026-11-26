namespace GestionProyectos.Api.Data.Entidades;

public class Proyecto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaFinEstimada { get; set; }

    public int IdEstado { get; set; }

    public int IdResponsable { get; set; }

    public DateTime FechaCreacion { get; set; }

    public Estado Estado { get; set; } = null!;

    public Usuario Responsable { get; set; } = null!;

    public ICollection<Tarea> Tareas { get; set; } = new List<Tarea>();
}
