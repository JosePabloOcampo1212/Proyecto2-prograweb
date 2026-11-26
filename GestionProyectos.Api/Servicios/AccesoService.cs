using GestionProyectos.Api.Data;
using GestionProyectos.Api.Data.Entidades;
using Microsoft.EntityFrameworkCore;

namespace GestionProyectos.Api.Servicios;

public class AccesoService(GestionDbContext db)
{
    public IQueryable<Proyecto> ProyectosVisibles(int idUsuario, bool esAdministrador) =>
        esAdministrador
            ? db.Proyectos
            : db.Proyectos.Where(p => p.IdResponsable == idUsuario || p.Tareas.Any(t => t.IdAsignado == idUsuario));

    public Task<bool> ParticipaAsync(int idProyecto, int idUsuario) =>
        db.Proyectos.AnyAsync(p => p.Id == idProyecto &&
            (p.IdResponsable == idUsuario || p.Tareas.Any(t => t.IdAsignado == idUsuario)));
}
