namespace GestionProyectos.Web.Servicios;

public static class SesionExtensiones
{
    public static bool EsAdministrador(this ISession sesion) =>
        sesion.GetString(SesionKeys.Rol) == SesionKeys.RolAdministrador;

    public static int IdUsuario(this ISession sesion) =>
        int.TryParse(sesion.GetString(SesionKeys.IdUsuario), out var id) ? id : 0;
}
