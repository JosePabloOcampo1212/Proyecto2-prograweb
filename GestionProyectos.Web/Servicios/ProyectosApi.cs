using GestionProyectos.Web.Models;

namespace GestionProyectos.Web.Servicios;

public class ProyectosApi(ApiClient api)
{
    public Task<RespuestaApi<List<ProyectoItem>>> ListarAsync() =>
        api.GetAsync<List<ProyectoItem>>("api/proyectos");

    public Task<RespuestaApi<ProyectoItem>> ObtenerAsync(int id) =>
        api.GetAsync<ProyectoItem>($"api/proyectos/{id}");

    public Task<RespuestaApi<ProyectoItem>> CrearAsync(ProyectoGuardar proyecto) =>
        api.PostAsync<ProyectoItem>("api/proyectos", proyecto);

    public Task<RespuestaApi<object>> ActualizarAsync(int id, ProyectoGuardar proyecto) =>
        api.PutAsync($"api/proyectos/{id}", proyecto);
}
