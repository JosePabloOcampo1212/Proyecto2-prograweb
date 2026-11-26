using System.Security.Claims;

namespace GestionProyectos.Api.Servicios;

public static class ClaimsPrincipalExtensiones
{
    public const string RolAdministrador = "Administrador";

    public static int IdUsuario(this ClaimsPrincipal usuario) =>
        int.Parse(usuario.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public static bool EsAdministrador(this ClaimsPrincipal usuario) =>
        usuario.IsInRole(RolAdministrador);
}
