using GestionProyectos.Api.Data;
using GestionProyectos.Api.Data.Entidades;
using GestionProyectos.Api.Dtos;
using GestionProyectos.Api.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionProyectos.Api.Controllers;

[Route("api/proyectos")]
public class ProyectosController(GestionDbContext db, AccesoService acceso) : ApiControllerBase
{
    private static IQueryable<ProyectoDto> Proyectar(IQueryable<Proyecto> consulta) =>
        consulta.Select(p => new ProyectoDto
        {
            Id = p.Id,
            Nombre = p.Nombre,
            Descripcion = p.Descripcion,
            FechaInicio = p.FechaInicio,
            FechaFinEstimada = p.FechaFinEstimada,
            IdEstado = p.IdEstado,
            Estado = p.Estado.Nombre,
            IdResponsable = p.IdResponsable,
            Responsable = p.Responsable.Nombre,
            FechaCreacion = p.FechaCreacion
        });

    [HttpGet]
    public async Task<ActionResult<List<ProyectoDto>>> Listar()
    {
        var visibles = acceso.ProyectosVisibles(User.IdUsuario(), User.EsAdministrador())
            .AsNoTracking()
            .OrderBy(p => p.Nombre);

        return await Proyectar(visibles).ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProyectoDto>> Obtener(int id)
    {
        var proyecto = await Proyectar(db.Proyectos.AsNoTracking().Where(p => p.Id == id)).FirstOrDefaultAsync();

        if (proyecto is null)
        {
            return NoEncontrado("El proyecto no existe.");
        }

        if (!User.EsAdministrador() && !await acceso.ParticipaAsync(id, User.IdUsuario()))
        {
            return Prohibido();
        }

        return proyecto;
    }

    [HttpPost]
    [Authorize(Roles = ClaimsPrincipalExtensiones.RolAdministrador)]
    public async Task<ActionResult<ProyectoDto>> Crear(ProyectoGuardarDto dto)
    {
        var error = await ValidarAsync(dto);

        if (error is not null)
        {
            return Peticion(error);
        }

        var proyecto = new Proyecto
        {
            Nombre = dto.Nombre.Trim(),
            Descripcion = dto.Descripcion?.Trim(),
            FechaInicio = dto.FechaInicio!.Value,
            FechaFinEstimada = dto.FechaFinEstimada!.Value,
            IdEstado = dto.IdEstado,
            IdResponsable = dto.IdResponsable
        };

        db.Proyectos.Add(proyecto);
        await db.SaveChangesAsync();

        var resultado = await Proyectar(db.Proyectos.AsNoTracking().Where(p => p.Id == proyecto.Id)).FirstAsync();
        return CreatedAtAction(nameof(Obtener), new { id = proyecto.Id }, resultado);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = ClaimsPrincipalExtensiones.RolAdministrador)]
    public async Task<IActionResult> Actualizar(int id, ProyectoGuardarDto dto)
    {
        var proyecto = await db.Proyectos.FirstOrDefaultAsync(p => p.Id == id);

        if (proyecto is null)
        {
            return NoEncontrado("El proyecto no existe.");
        }

        var error = await ValidarAsync(dto);

        if (error is not null)
        {
            return Peticion(error);
        }

        proyecto.Nombre = dto.Nombre.Trim();
        proyecto.Descripcion = dto.Descripcion?.Trim();
        proyecto.FechaInicio = dto.FechaInicio!.Value;
        proyecto.FechaFinEstimada = dto.FechaFinEstimada!.Value;
        proyecto.IdEstado = dto.IdEstado;
        proyecto.IdResponsable = dto.IdResponsable;

        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = ClaimsPrincipalExtensiones.RolAdministrador)]
    public async Task<IActionResult> Cerrar(int id)
    {
        var proyecto = await db.Proyectos.FirstOrDefaultAsync(p => p.Id == id);

        if (proyecto is null)
        {
            return NoEncontrado("El proyecto no existe.");
        }

        proyecto.IdEstado = await db.Estados
            .Where(e => e.Nombre == "Cerrado" && e.EntidadAplicacion == "Proyecto")
            .Select(e => e.Id)
            .FirstAsync();

        await db.SaveChangesAsync();
        return NoContent();
    }

    private async Task<string?> ValidarAsync(ProyectoGuardarDto dto)
    {
        if (dto.FechaFinEstimada < dto.FechaInicio)
        {
            return "La fecha estimada de fin no puede ser anterior a la fecha de inicio.";
        }

        if (!await db.Estados.AnyAsync(e => e.Id == dto.IdEstado && e.EntidadAplicacion == "Proyecto"))
        {
            return "El estado seleccionado no es válido para un proyecto.";
        }

        if (!await db.Usuarios.AnyAsync(u => u.Id == dto.IdResponsable && u.Activo))
        {
            return "El responsable seleccionado no existe o está inactivo.";
        }

        return null;
    }
}
