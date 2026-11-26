namespace GestionProyectos.Api.Data.Entidades;

public class Prioridad
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public ICollection<Tarea> Tareas { get; set; } = new List<Tarea>();
}
