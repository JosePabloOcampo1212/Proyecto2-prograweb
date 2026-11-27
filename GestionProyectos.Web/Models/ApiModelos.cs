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

public record TareaItem(
    int Id,
    int IdProyecto,
    string Titulo,
    string? Descripcion,
    int IdAsignado,
    string Asignado,
    int IdEstado,
    string Estado,
    int IdPrioridad,
    string Prioridad,
    DateTime FechaCreacion,
    DateOnly FechaVencimiento);

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

public record TareaGuardar(
    int IdProyecto,
    string Titulo,
    string? Descripcion,
    int IdAsignado,
    int IdEstado,
    int IdPrioridad,
    DateOnly FechaVencimiento);
