namespace GestionProyectos.Api.Dtos;

public class CatalogoDto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;
}

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
