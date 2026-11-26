using GestionProyectos.Web.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GestionProyectos.Web.Filtros;

public class SesionRequeridaAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext contexto)
    {
        if (contexto.ActionDescriptor.EndpointMetadata.OfType<IAllowAnonymous>().Any())
        {
            return;
        }

        if (string.IsNullOrEmpty(contexto.HttpContext.Session.GetString(SesionKeys.Token)))
        {
            contexto.Result = new RedirectToActionResult("Login", "Cuenta", null);
        }
    }
}
