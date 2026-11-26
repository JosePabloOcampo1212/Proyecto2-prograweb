using GestionProyectos.Api.Data;
using GestionProyectos.Api.Dtos;
using GestionProyectos.Api.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionProyectos.Api.Controllers;

[Route("api/dashboard")]
public class DashboardController(GestionDbContext db, AccesoService acceso) : ApiControllerBase
{
    [HttpGet("indicadores")]
    public async Task<ActionResult<IndicadoresDto>> Indicadores()
    {
        var idUsuario = User.IdUsuario();
        var proyectos = acceso.ProyectosVisibles(idUsuario, User.EsAdministrador()).AsNoTracking();
        var idsProyectos = proyectos.Select(p => p.Id);

        var proyectosActivos = await proyectos.CountAsync(p => p.Estado.Nombre == "Activo");

        var estados = await db.Estados
            .AsNoTracking()
            .Where(e => e.EntidadAplicacion == "Tarea")
            .OrderBy(e => e.Id)
            .ToListAsync();

        var conteos = await db.Tareas
            .AsNoTracking()
            .Where(t => idsProyectos.Contains(t.IdProyecto))
            .GroupBy(t => t.IdEstado)
            .Select(g => new { IdEstado = g.Key, Cantidad = g.Count() })
            .ToListAsync();

        var tareasAsignadas = await db.Tareas.CountAsync(t => t.IdAsignado == idUsuario);

        return new IndicadoresDto
        {
            ProyectosActivos = proyectosActivos,
            TareasPorEstado = estados
                .Select(e => new ConteoEstadoDto
                {
                    Estado = e.Nombre,
                    Cantidad = conteos.FirstOrDefault(c => c.IdEstado == e.Id)?.Cantidad ?? 0
                })
                .ToList(),
            TareasAsignadasAMi = tareasAsignadas
        };
    }
}
