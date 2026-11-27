using GestionProyectos.Web.Models;

namespace GestionProyectos.Web.Servicios;

public class TareasApi(ApiClient api)
{
    public Task<RespuestaApi<List<TareaItem>>> ListarPorProyectoAsync(int idProyecto) =>
        api.GetAsync<List<TareaItem>>($"api/tareas?idProyecto={idProyecto}");

    public Task<RespuestaApi<TareaItem>> ObtenerAsync(int id) =>
        api.GetAsync<TareaItem>($"api/tareas/{id}");

    public Task<RespuestaApi<TareaItem>> CrearAsync(TareaGuardar tarea) =>
        api.PostAsync<TareaItem>("api/tareas", tarea);

    public Task<RespuestaApi<object>> ActualizarAsync(int id, TareaGuardar tarea) =>
        api.PutAsync($"api/tareas/{id}", tarea);
}
