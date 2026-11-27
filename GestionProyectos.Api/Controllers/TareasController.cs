using GestionProyectos.Api.Data;
using GestionProyectos.Api.Data.Entidades;
using GestionProyectos.Api.Dtos;
using GestionProyectos.Api.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionProyectos.Api.Controllers;

[Route("api/tareas")]
public class TareasController(GestionDbContext db, AccesoService acceso) : ApiControllerBase
{
    private static IQueryable<TareaDto> Proyectar(IQueryable<Tarea> consulta) =>
        consulta.Select(t => new TareaDto
        {
            Id = t.Id,
            IdProyecto = t.IdProyecto,
            Titulo = t.Titulo,
            Descripcion = t.Descripcion,
            IdAsignado = t.IdAsignado,
            Asignado = t.Asignado.Nombre,
            IdEstado = t.IdEstado,
            Estado = t.Estado.Nombre,
            IdPrioridad = t.IdPrioridad,
            Prioridad = t.Prioridad.Nombre,
            FechaCreacion = t.FechaCreacion,
            FechaVencimiento = t.FechaVencimiento
        });

    [HttpGet]
    public async Task<ActionResult<List<TareaDto>>> Listar([FromQuery] int idProyecto)
    {
        if (!await db.Proyectos.AnyAsync(p => p.Id == idProyecto))
        {
            return NoEncontrado("El proyecto no existe.");
        }

        if (!User.EsAdministrador() && !await acceso.ParticipaAsync(idProyecto, User.IdUsuario()))
        {
            return Prohibido();
        }

        var tareas = db.Tareas.AsNoTracking().Where(t => t.IdProyecto == idProyecto).OrderBy(t => t.Id);
        return await Proyectar(tareas).ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TareaDto>> Obtener(int id)
    {
        var tarea = await Proyectar(db.Tareas.AsNoTracking().Where(t => t.Id == id)).FirstOrDefaultAsync();

        if (tarea is null)
        {
            return NoEncontrado("La tarea no existe.");
        }

        if (!User.EsAdministrador() && !await acceso.ParticipaAsync(tarea.IdProyecto, User.IdUsuario()))
        {
            return Prohibido();
        }

        return tarea;
    }

    [HttpPost]
    public async Task<ActionResult<TareaDto>> Crear(TareaGuardarDto dto)
    {
        if (!await db.Proyectos.AnyAsync(p => p.Id == dto.IdProyecto))
        {
            return Peticion("El proyecto indicado no existe.");
        }

        if (!User.EsAdministrador() && !await acceso.ParticipaAsync(dto.IdProyecto, User.IdUsuario()))
        {
            return Prohibido();
        }

        var error = await ValidarAsync(dto);

        if (error is not null)
        {
            return Peticion(error);
        }

        var tarea = new Tarea
        {
            IdProyecto = dto.IdProyecto,
            Titulo = dto.Titulo.Trim(),
            Descripcion = dto.Descripcion?.Trim(),
            IdAsignado = dto.IdAsignado,
            IdEstado = dto.IdEstado,
            IdPrioridad = dto.IdPrioridad,
            FechaVencimiento = dto.FechaVencimiento!.Value
        };

        db.Tareas.Add(tarea);
        await db.SaveChangesAsync();

        var resultado = await Proyectar(db.Tareas.AsNoTracking().Where(t => t.Id == tarea.Id)).FirstAsync();
        return CreatedAtAction(nameof(Obtener), new { id = tarea.Id }, resultado);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, TareaGuardarDto dto)
    {
        var tarea = await db.Tareas.FirstOrDefaultAsync(t => t.Id == id);

        if (tarea is null)
        {
            return NoEncontrado("La tarea no existe.");
        }

        if (!User.EsAdministrador())
        {
            if (tarea.IdAsignado != User.IdUsuario())
            {
                return Prohibido("Solo puede modificar las tareas que tiene asignadas.");
            }

            dto.IdProyecto = tarea.IdProyecto;
            dto.Titulo = tarea.Titulo;
            dto.Descripcion = tarea.Descripcion;
            dto.IdAsignado = tarea.IdAsignado;
            dto.FechaVencimiento = tarea.FechaVencimiento;
        }

        if (!await db.Proyectos.AnyAsync(p => p.Id == dto.IdProyecto))
        {
            return Peticion("El proyecto indicado no existe.");
        }

        var error = await ValidarAsync(dto);

        if (error is not null)
        {
            return Peticion(error);
        }

        tarea.IdProyecto = dto.IdProyecto;
        tarea.Titulo = dto.Titulo.Trim();
        tarea.Descripcion = dto.Descripcion?.Trim();
        tarea.IdAsignado = dto.IdAsignado;
        tarea.IdEstado = dto.IdEstado;
        tarea.IdPrioridad = dto.IdPrioridad;
        tarea.FechaVencimiento = dto.FechaVencimiento!.Value;

        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = ClaimsPrincipalExtensiones.RolAdministrador)]
    public async Task<IActionResult> Eliminar(int id)
    {
        var tarea = await db.Tareas.FirstOrDefaultAsync(t => t.Id == id);

        if (tarea is null)
        {
            return NoEncontrado("La tarea no existe.");
        }

        db.Tareas.Remove(tarea);
        await db.SaveChangesAsync();
        return NoContent();
    }

    private async Task<string?> ValidarAsync(TareaGuardarDto dto)
    {
        if (!await db.Usuarios.AnyAsync(u => u.Id == dto.IdAsignado && u.Activo))
        {
            return "El usuario asignado no existe o está inactivo.";
        }

        if (!await db.Estados.AnyAsync(e => e.Id == dto.IdEstado && e.EntidadAplicacion == "Tarea"))
        {
            return "El estado seleccionado no es válido para una tarea.";
        }

        if (!await db.Prioridades.AnyAsync(p => p.Id == dto.IdPrioridad))
        {
            return "La prioridad seleccionada no existe.";
        }

        return null;
    }
}
