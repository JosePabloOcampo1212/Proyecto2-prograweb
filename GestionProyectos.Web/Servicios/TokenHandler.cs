using System.Net.Http.Headers;

namespace GestionProyectos.Web.Servicios;

public class TokenHandler(IHttpContextAccessor accesor) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage solicitud, CancellationToken cancelacion)
    {
        var token = accesor.HttpContext?.Session.GetString(SesionKeys.Token);

        if (!string.IsNullOrEmpty(token))
        {
            solicitud.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return base.SendAsync(solicitud, cancelacion);
    }
}
