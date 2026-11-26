namespace GestionProyectos.Api.Data.Entidades;

public class Estado
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string EntidadAplicacion { get; set; } = null!;

    public ICollection<Proyecto> Proyectos { get; set; } = new List<Proyecto>();

    public ICollection<Tarea> Tareas { get; set; } = new List<Tarea>();
}
