namespace GestionProyectos.Web.Models;

public record LoginRespuesta(string Token, DateTime Expira);

public record MeRespuesta(int Id, string Nombre, string Email, string Rol);

public record ConteoEstadoItem(string Estado, int Cantidad);

public record IndicadoresItem(int ProyectosActivos, List<ConteoEstadoItem> TareasPorEstado, int TareasAsignadasAMi);

public record LoginSolicitud(string Email, string Contrasena);
