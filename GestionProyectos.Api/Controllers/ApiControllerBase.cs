using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionProyectos.Api.Controllers;

[ApiController]
[Authorize]
public abstract class ApiControllerBase : ControllerBase
{
    protected ObjectResult Peticion(string mensaje) =>
        StatusCode(StatusCodes.Status400BadRequest, new { mensaje });

    protected ObjectResult Prohibido(string mensaje = "No tiene permiso para realizar esta operación.") =>
        StatusCode(StatusCodes.Status403Forbidden, new { mensaje });

    protected ObjectResult NoEncontrado(string mensaje) =>
        StatusCode(StatusCodes.Status404NotFound, new { mensaje });

    protected ObjectResult Conflicto(string mensaje) =>
        StatusCode(StatusCodes.Status409Conflict, new { mensaje });
}
