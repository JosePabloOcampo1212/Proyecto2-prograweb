namespace GestionProyectos.Api.Dtos;

public class ConteoEstadoDto
{
    public string Estado { get; set; } = null!;

    public int Cantidad { get; set; }
}

public class IndicadoresDto
{
    public int ProyectosActivos { get; set; }

    public List<ConteoEstadoDto> TareasPorEstado { get; set; } = new();

    public int TareasAsignadasAMi { get; set; }
}
