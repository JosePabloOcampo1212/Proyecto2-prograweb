using System.Net;
using GestionProyectos.Web.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace GestionProyectos.Web.Controllers;

public abstract class BaseController : Controller
{
    protected IActionResult? ErrorGeneral<T>(RespuestaApi<T> respuesta)
    {
        switch (respuesta.Codigo)
        {
            case HttpStatusCode.Unauthorized:
                HttpContext.Session.Clear();
                TempData["Error"] = "La sesión expiró. Inicie sesión de nuevo.";
                return RedirectToAction("Login", "Cuenta");
            case HttpStatusCode.Forbidden:
                TempData["Error"] = respuesta.Mensaje ?? "No tiene permiso para realizar esta operación.";
                return RedirectToAction("Index", "Dashboard");
            case HttpStatusCode.NotFound:
                return NotFound();
            default:
                return null;
        }
    }

    protected IActionResult ErrorServicio<T>(RespuestaApi<T> respuesta) =>
        ErrorGeneral(respuesta)
        ?? View("Error", respuesta.Mensaje ?? "Ocurrió un error al comunicarse con el servicio.");

    protected static bool EsErrorDeValidacion<T>(RespuestaApi<T> respuesta) =>
        respuesta.Codigo is HttpStatusCode.BadRequest or HttpStatusCode.Conflict;
}
