using GestionProyectos.Api.Data;
using GestionProyectos.Api.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionProyectos.Api.Controllers;

[Route("api")]
public class CatalogosController(GestionDbContext db) : ApiControllerBase
{
    [HttpGet("roles")]
    public async Task<ActionResult<List<CatalogoDto>>> Roles() =>
        await db.Roles
            .AsNoTracking()
            .OrderBy(r => r.Id)
            .Select(r => new CatalogoDto { Id = r.Id, Nombre = r.Nombre })
            .ToListAsync();

    [HttpGet("estados")]
    public async Task<ActionResult<List<CatalogoDto>>> Estados([FromQuery] string entidad)
    {
        if (entidad != "Proyecto" && entidad != "Tarea")
        {
            return Peticion("La entidad debe ser Proyecto o Tarea.");
        }

        return await db.Estados
            .AsNoTracking()
            .Where(e => e.EntidadAplicacion == entidad)
            .OrderBy(e => e.Id)
            .Select(e => new CatalogoDto { Id = e.Id, Nombre = e.Nombre })
            .ToListAsync();
    }

    [HttpGet("prioridades")]
    public async Task<ActionResult<List<CatalogoDto>>> Prioridades() =>
        await db.Prioridades
            .AsNoTracking()
            .OrderBy(p => p.Id)
            .Select(p => new CatalogoDto { Id = p.Id, Nombre = p.Nombre })
            .ToListAsync();
}
