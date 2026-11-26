using GestionProyectos.Web.Models;

namespace GestionProyectos.Web.Servicios;

public class UsuariosApi(ApiClient api)
{
    public Task<RespuestaApi<List<UsuarioItem>>> ListarAsync() =>
        api.GetAsync<List<UsuarioItem>>("api/usuarios");

    public Task<RespuestaApi<UsuarioItem>> ObtenerAsync(int id) =>
        api.GetAsync<UsuarioItem>($"api/usuarios/{id}");

    public Task<RespuestaApi<UsuarioItem>> CrearAsync(UsuarioGuardar usuario) =>
        api.PostAsync<UsuarioItem>("api/usuarios", usuario);

    public Task<RespuestaApi<object>> ActualizarAsync(int id, UsuarioGuardar usuario) =>
        api.PutAsync($"api/usuarios/{id}", usuario);

    public Task<RespuestaApi<object>> DesactivarAsync(int id) =>
        api.DeleteAsync($"api/usuarios/{id}");
}
