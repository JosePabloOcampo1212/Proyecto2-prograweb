using GestionProyectos.Web.Models;

namespace GestionProyectos.Web.Servicios;

public class DashboardApi(ApiClient api)
{
    public Task<RespuestaApi<IndicadoresItem>> IndicadoresAsync() =>
        api.GetAsync<IndicadoresItem>("api/dashboard/indicadores");
}
