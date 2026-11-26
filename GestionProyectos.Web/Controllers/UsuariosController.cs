using GestionProyectos.Web.Filtros;
using GestionProyectos.Web.Models;
using GestionProyectos.Web.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace GestionProyectos.Web.Controllers;

[SoloAdministrador]
public class UsuariosController(UsuariosApi usuarios, CatalogosApi catalogos) : BaseController
{
    public async Task<IActionResult> Index()
    {
        var lista = await usuarios.ListarAsync();

        if (!lista.Exito)
        {
            return ErrorServicio(lista);
        }

        return View(lista.Datos);
    }

    [HttpGet]
    public async Task<IActionResult> Crear()
    {
        var modelo = new UsuarioFormViewModel();
        var error = await CargarRolesAsync(modelo);
        return error ?? View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(UsuarioFormViewModel modelo)
    {
        if (string.IsNullOrWhiteSpace(modelo.Contrasena))
        {
            ModelState.AddModelError(nameof(modelo.Contrasena), "La contraseña es obligatoria.");
        }

        if (ModelState.IsValid)
        {
            var respuesta = await usuarios.CrearAsync(Guardar(modelo));

            if (respuesta.Exito)
            {
                TempData["Exito"] = "El usuario se creó correctamente.";
                return RedirectToAction(nameof(Index));
            }

            if (!EsErrorDeValidacion(respuesta))
            {
                return ErrorServicio(respuesta);
            }

            ModelState.AddModelError("", respuesta.Mensaje ?? "No fue posible guardar el usuario.");
        }

        var error = await CargarRolesAsync(modelo);
        return error ?? View(modelo);
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var usuario = await usuarios.ObtenerAsync(id);

        if (!usuario.Exito)
        {
            return ErrorServicio(usuario);
        }

        var datos = usuario.Datos!;
        var modelo = new UsuarioFormViewModel
        {
            Id = datos.Id,
            Nombre = datos.Nombre,
            Email = datos.Email,
            IdRol = datos.IdRol,
            Activo = datos.Activo
        };

        var error = await CargarRolesAsync(modelo);
        return error ?? View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(UsuarioFormViewModel modelo)
    {
        if (ModelState.IsValid)
        {
            var respuesta = await usuarios.ActualizarAsync(modelo.Id, Guardar(modelo));

            if (respuesta.Exito)
            {
                TempData["Exito"] = "El usuario se actualizó correctamente.";
                return RedirectToAction(nameof(Index));
            }

            if (!EsErrorDeValidacion(respuesta))
            {
                return ErrorServicio(respuesta);
            }

            ModelState.AddModelError("", respuesta.Mensaje ?? "No fue posible guardar el usuario.");
        }

        var error = await CargarRolesAsync(modelo);
        return error ?? View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Desactivar(int id)
    {
        var respuesta = await usuarios.DesactivarAsync(id);

        if (!respuesta.Exito)
        {
            return ErrorServicio(respuesta);
        }

        TempData["Exito"] = "El usuario se desactivó correctamente.";
        return RedirectToAction(nameof(Index));
    }

    private static UsuarioGuardar Guardar(UsuarioFormViewModel modelo) =>
        new(modelo.Nombre, modelo.Email, modelo.Contrasena, modelo.IdRol, modelo.Activo);

    private async Task<IActionResult?> CargarRolesAsync(UsuarioFormViewModel modelo)
    {
        var roles = await catalogos.RolesAsync();

        if (!roles.Exito)
        {
            return ErrorServicio(roles);
        }

        modelo.Roles = roles.Datos!;
        return null;
    }
}
