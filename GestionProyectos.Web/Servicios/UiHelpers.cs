namespace GestionProyectos.Web.Servicios;

public static class UiHelpers
{
    public static string Iniciales(string? nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            return "?";
        }

        var partes = nombre.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var iniciales = partes.Length == 1
            ? partes[0][..Math.Min(2, partes[0].Length)]
            : $"{partes[0][0]}{partes[^1][0]}";
        return iniciales.ToUpperInvariant();
    }

    // Color estable por persona, para que el avatar siempre tenga el mismo tono.
    public static int TonoAvatar(string? nombre)
    {
        var suma = 0;
        foreach (var c in nombre ?? string.Empty)
        {
            suma += c;
        }
        return suma % 6;
    }

    public static string ClaseEstado(string? estado) => estado switch
    {
        "Activo" => "estado--activo",
        "Cerrado" => "estado--cerrado",
        "Pendiente" => "estado--pendiente",
        "En progreso" => "estado--progreso",
        "Completada" => "estado--completada",
        "Cancelada" => "estado--cancelada",
        _ => "estado--neutro"
    };

    public static string ClasePrioridad(string? prioridad) => prioridad switch
    {
        "Alta" => "prioridad--alta",
        "Media" => "prioridad--media",
        _ => "prioridad--baja"
    };

    public static bool EstaVencida(DateOnly fecha, string? estado) =>
        fecha < DateOnly.FromDateTime(DateTime.Today) && estado is not ("Completada" or "Cancelada");

    private static readonly string[] Meses =
        ["ene", "feb", "mar", "abr", "may", "jun", "jul", "ago", "sep", "oct", "nov", "dic"];

    public static string FechaCorta(DateOnly fecha) => $"{fecha.Day:00} {Meses[fecha.Month - 1]} {fecha.Year}";

    public static string FechaCorta(DateTime fecha) => FechaCorta(DateOnly.FromDateTime(fecha));
}
