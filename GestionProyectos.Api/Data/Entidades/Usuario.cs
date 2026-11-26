namespace GestionProyectos.Api.Data.Entidades;

public class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string ContrasenaHash { get; set; } = null!;

    public int IdRol { get; set; }

    public DateTime FechaRegistro { get; set; }

    public bool Activo { get; set; }

    public Rol Rol { get; set; } = null!;

    public ICollection<Proyecto> ProyectosResponsable { get; set; } = new List<Proyecto>();

    public ICollection<Tarea> TareasAsignadas { get; set; } = new List<Tarea>();
}
