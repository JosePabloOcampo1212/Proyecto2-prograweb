using GestionProyectos.Web.Models;
using GestionProyectos.Web.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace GestionProyectos.Web.Controllers;

public class TareasController(
    TareasApi tareas,
    ProyectosApi proyectos,
    UsuariosApi usuarios,
    CatalogosApi catalogos) : BaseController
{
    public async Task<IActionResult> Index(int idProyecto)
    {
        var proyecto = await proyectos.ObtenerAsync(idProyecto);

        if (!proyecto.Exito)
        {
            return ErrorServicio(proyecto);
        }

        var lista = await tareas.ListarPorProyectoAsync(idProyecto);

        if (!lista.Exito)
        {
            return ErrorServicio(lista);
        }

        return View(new TareasIndexViewModel { Proyecto = proyecto.Datos!, Tareas = lista.Datos! });
    }

    [HttpGet]
    public async Task<IActionResult> Crear(int idProyecto)
    {
        var modelo = new TareaFormViewModel { IdProyecto = idProyecto };
        var error = await CargarCrearAsync(modelo);

        if (error is not null)
        {
            return error;
        }

        modelo.IdEstado = modelo.Estados.FirstOrDefault()?.Id ?? 0;
        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(TareaFormViewModel modelo)
    {
        if (ModelState.IsValid)
        {
            var guardar = new TareaGuardar(
                modelo.IdProyecto,
                modelo.Titulo,
                modelo.Descripcion,
                modelo.IdAsignado,
                modelo.IdEstado,
                modelo.IdPrioridad,
                modelo.FechaVencimiento!.Value);

            var respuesta = await tareas.CrearAsync(guardar);

            if (respuesta.Exito)
            {
                TempData["Exito"] = "La tarea se creó correctamente.";
                return RedirectToAction(nameof(Index), new { idProyecto = modelo.IdProyecto });
            }

            if (!EsErrorDeValidacion(respuesta))
            {
                return ErrorServicio(respuesta);
            }

            ModelState.AddModelError("", respuesta.Mensaje ?? "No fue posible guardar la tarea.");
        }

        var error = await CargarCrearAsync(modelo);
        return error ?? View(modelo);
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var modelo = new TareaEditarViewModel { Id = id };
        var error = await CargarEditarAsync(modelo, conservarSeleccion: false);
        return error ?? View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(TareaEditarViewModel modelo)
    {
        var actual = await tareas.ObtenerAsync(modelo.Id);

        if (!actual.Exito)
        {
            return ErrorServicio(actual);
        }

        if (ModelState.IsValid)
        {
            var tarea = actual.Datos!;
            var guardar = new TareaGuardar(
                tarea.IdProyecto,
                tarea.Titulo,
                tarea.Descripcion,
                tarea.IdAsignado,
                modelo.IdEstado,
                modelo.IdPrioridad,
                tarea.FechaVencimiento);

            var respuesta = await tareas.ActualizarAsync(modelo.Id, guardar);

            if (respuesta.Exito)
            {
                TempData["Exito"] = "La tarea se actualizó correctamente.";
                return RedirectToAction(nameof(Index), new { idProyecto = tarea.IdProyecto });
            }

            if (!EsErrorDeValidacion(respuesta))
            {
                return ErrorServicio(respuesta);
            }

            ModelState.AddModelError("", respuesta.Mensaje ?? "No fue posible guardar la tarea.");
        }

        var error = await CargarEditarAsync(modelo, conservarSeleccion: true);
        return error ?? View(modelo);
    }

    private async Task<IActionResult?> CargarCrearAsync(TareaFormViewModel modelo)
    {
        var proyecto = await proyectos.ObtenerAsync(modelo.IdProyecto);

        if (!proyecto.Exito)
        {
            return ErrorServicio(proyecto);
        }

        var estados = await catalogos.EstadosAsync("Tarea");

        if (!estados.Exito)
        {
            return ErrorServicio(estados);
        }

        var prioridades = await catalogos.PrioridadesAsync();

        if (!prioridades.Exito)
        {
            return ErrorServicio(prioridades);
        }

        var asignables = await usuarios.ListarAsync();

        if (!asignables.Exito)
        {
            return ErrorServicio(asignables);
        }

        modelo.NombreProyecto = proyecto.Datos!.Nombre;
        modelo.Estados = estados.Datos!;
        modelo.Prioridades = prioridades.Datos!;
        modelo.Usuarios = asignables.Datos!
            .Where(u => u.Activo)
            .Select(u => new CatalogoItem(u.Id, u.Nombre))
            .ToList();

        return null;
    }

    private async Task<IActionResult?> CargarEditarAsync(TareaEditarViewModel modelo, bool conservarSeleccion)
    {
        var tarea = await tareas.ObtenerAsync(modelo.Id);

        if (!tarea.Exito)
        {
            return ErrorServicio(tarea);
        }

        var proyecto = await proyectos.ObtenerAsync(tarea.Datos!.IdProyecto);

        if (!proyecto.Exito)
        {
            return ErrorServicio(proyecto);
        }

        var estados = await catalogos.EstadosAsync("Tarea");

        if (!estados.Exito)
        {
            return ErrorServicio(estados);
        }

        var prioridades = await catalogos.PrioridadesAsync();

        if (!prioridades.Exito)
        {
            return ErrorServicio(prioridades);
        }

        var datos = tarea.Datos;
        modelo.IdProyecto = datos.IdProyecto;
        modelo.NombreProyecto = proyecto.Datos!.Nombre;
        modelo.Titulo = datos.Titulo;
        modelo.Descripcion = datos.Descripcion;
        modelo.Asignado = datos.Asignado;
        modelo.FechaVencimiento = datos.FechaVencimiento;
        modelo.Estados = estados.Datos!;
        modelo.Prioridades = prioridades.Datos!;

        if (!conservarSeleccion)
        {
            modelo.IdEstado = datos.IdEstado;
            modelo.IdPrioridad = datos.IdPrioridad;
        }

        return null;
    }
}
