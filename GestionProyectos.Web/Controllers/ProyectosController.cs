using GestionProyectos.Web.Filtros;
using GestionProyectos.Web.Models;
using GestionProyectos.Web.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace GestionProyectos.Web.Controllers;

public class ProyectosController(ProyectosApi proyectos, UsuariosApi usuarios, CatalogosApi catalogos) : BaseController
{
    public async Task<IActionResult> Index()
    {
        var lista = await proyectos.ListarAsync();

        if (!lista.Exito)
        {
            return ErrorServicio(lista);
        }

        return View(lista.Datos);
    }

    public async Task<IActionResult> Detalle(int id)
    {
        var proyecto = await proyectos.ObtenerAsync(id);

        if (!proyecto.Exito)
        {
            return ErrorServicio(proyecto);
        }

        return View(proyecto.Datos);
    }

    [SoloAdministrador]
    [HttpGet]
    public async Task<IActionResult> Crear()
    {
        var modelo = new ProyectoFormViewModel();
        var error = await CargarCombosAsync(modelo);

        if (error is not null)
        {
            return error;
        }

        modelo.IdEstado = modelo.Estados.FirstOrDefault()?.Id ?? 0;
        return View(modelo);
    }

    [SoloAdministrador]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(ProyectoFormViewModel modelo)
    {
        if (ModelState.IsValid)
        {
            var respuesta = await proyectos.CrearAsync(Guardar(modelo));

            if (respuesta.Exito)
            {
                TempData["Exito"] = "El proyecto se creó correctamente.";
                return RedirectToAction(nameof(Index));
            }

            if (!EsErrorDeValidacion(respuesta))
            {
                return ErrorServicio(respuesta);
            }

            ModelState.AddModelError("", respuesta.Mensaje ?? "No fue posible guardar el proyecto.");
        }

        var error = await CargarCombosAsync(modelo);
        return error ?? View(modelo);
    }

    [SoloAdministrador]
    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var proyecto = await proyectos.ObtenerAsync(id);

        if (!proyecto.Exito)
        {
            return ErrorServicio(proyecto);
        }

        var datos = proyecto.Datos!;
        var modelo = new ProyectoFormViewModel
        {
            Id = datos.Id,
            Nombre = datos.Nombre,
            Descripcion = datos.Descripcion,
            FechaInicio = datos.FechaInicio,
            FechaFinEstimada = datos.FechaFinEstimada,
            IdEstado = datos.IdEstado,
            IdResponsable = datos.IdResponsable
        };

        var error = await CargarCombosAsync(modelo);
        return error ?? View(modelo);
    }

    [SoloAdministrador]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(ProyectoFormViewModel modelo)
    {
        if (ModelState.IsValid)
        {
            var respuesta = await proyectos.ActualizarAsync(modelo.Id, Guardar(modelo));

            if (respuesta.Exito)
            {
                TempData["Exito"] = "El proyecto se actualizó correctamente.";
                return RedirectToAction(nameof(Index));
            }

            if (!EsErrorDeValidacion(respuesta))
            {
                return ErrorServicio(respuesta);
            }

            ModelState.AddModelError("", respuesta.Mensaje ?? "No fue posible guardar el proyecto.");
        }

        var error = await CargarCombosAsync(modelo);
        return error ?? View(modelo);
    }

    private static ProyectoGuardar Guardar(ProyectoFormViewModel modelo) =>
        new(
            modelo.Nombre,
            modelo.Descripcion,
            modelo.FechaInicio!.Value,
            modelo.FechaFinEstimada!.Value,
            modelo.IdEstado,
            modelo.IdResponsable);

    private async Task<IActionResult?> CargarCombosAsync(ProyectoFormViewModel modelo)
    {
        var estados = await catalogos.EstadosAsync("Proyecto");

        if (!estados.Exito)
        {
            return ErrorServicio(estados);
        }

        var responsables = await usuarios.ListarAsync();

        if (!responsables.Exito)
        {
            return ErrorServicio(responsables);
        }

        modelo.Estados = estados.Datos!;
        modelo.Responsables = responsables.Datos!
            .Where(u => u.Activo)
            .Select(u => new CatalogoItem(u.Id, u.Nombre))
            .ToList();

        return null;
    }
}
