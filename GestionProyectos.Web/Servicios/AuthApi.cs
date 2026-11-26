using GestionProyectos.Web.Models;

namespace GestionProyectos.Web.Servicios;

public class AuthApi(ApiClient api)
{
    public Task<RespuestaApi<LoginRespuesta>> LoginAsync(string email, string contrasena) =>
        api.PostAsync<LoginRespuesta>("api/auth/login", new LoginSolicitud(email, contrasena));

    public Task<RespuestaApi<MeRespuesta>> MeAsync() =>
        api.GetAsync<MeRespuesta>("api/auth/me");
}
