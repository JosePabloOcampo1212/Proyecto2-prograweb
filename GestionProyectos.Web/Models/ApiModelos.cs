namespace GestionProyectos.Web.Models;

public record LoginRespuesta(string Token, DateTime Expira);

public record MeRespuesta(int Id, string Nombre, string Email, string Rol);

public record CatalogoItem(int Id, string Nombre);

public record UsuarioItem(int Id, string Nombre, string Email, int IdRol, string Rol, DateTime FechaRegistro, bool Activo);

public record ProyectoItem(
    int Id,
    string Nombre,
    string? Descripcion,
    DateOnly FechaInicio,
    DateOnly FechaFinEstimada,
    int IdEstado,
    string Estado,
    int IdResponsable,
    string Responsable,
    DateTime FechaCreacion);

public record ConteoEstadoItem(string Estado, int Cantidad);

public record IndicadoresItem(int ProyectosActivos, List<ConteoEstadoItem> TareasPorEstado, int TareasAsignadasAMi);

public record LoginSolicitud(string Email, string Contrasena);

public record UsuarioGuardar(string Nombre, string Email, string? Contrasena, int IdRol, bool Activo);

public record ProyectoGuardar(
    string Nombre,
    string? Descripcion,
    DateOnly FechaInicio,
    DateOnly FechaFinEstimada,
    int IdEstado,
    int IdResponsable);
