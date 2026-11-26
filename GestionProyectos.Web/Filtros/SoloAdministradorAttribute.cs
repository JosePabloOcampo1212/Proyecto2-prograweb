using GestionProyectos.Web.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GestionProyectos.Web.Filtros;

public class SoloAdministradorAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext contexto)
    {
        if (contexto.HttpContext.Session.GetString(SesionKeys.Rol) == SesionKeys.RolAdministrador)
        {
            return;
        }

        if (contexto.Controller is Controller controlador)
        {
            controlador.TempData["Error"] = "No tiene permiso para acceder a esa sección.";
        }

        contexto.Result = new RedirectToActionResult("Index", "Dashboard", null);
    }
}
