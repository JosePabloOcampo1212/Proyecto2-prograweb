using GestionProyectos.Web.Models;

namespace GestionProyectos.Web.Servicios;

public class CatalogosApi(ApiClient api)
{
    public Task<RespuestaApi<List<CatalogoItem>>> RolesAsync() =>
        api.GetAsync<List<CatalogoItem>>("api/roles");

    public Task<RespuestaApi<List<CatalogoItem>>> EstadosAsync(string entidad) =>
        api.GetAsync<List<CatalogoItem>>($"api/estados?entidad={entidad}");

    public Task<RespuestaApi<List<CatalogoItem>>> PrioridadesAsync() =>
        api.GetAsync<List<CatalogoItem>>("api/prioridades");
}
