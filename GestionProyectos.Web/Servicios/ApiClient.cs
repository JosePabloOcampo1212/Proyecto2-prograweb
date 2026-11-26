using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace GestionProyectos.Web.Servicios;

public class RespuestaApi<T>
{
    public HttpStatusCode Codigo { get; init; }

    public T? Datos { get; init; }

    public string? Mensaje { get; init; }

    public bool Exito => (int)Codigo is >= 200 and < 300;
}

public class ApiClient(HttpClient http)
{
    private static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web);

    public Task<RespuestaApi<T>> GetAsync<T>(string ruta) =>
        EnviarAsync<T>(new HttpRequestMessage(HttpMethod.Get, ruta));

    public Task<RespuestaApi<T>> PostAsync<T>(string ruta, object cuerpo) =>
        EnviarAsync<T>(ConCuerpo(HttpMethod.Post, ruta, cuerpo));

    public Task<RespuestaApi<object>> PutAsync(string ruta, object cuerpo) =>
        EnviarAsync<object>(ConCuerpo(HttpMethod.Put, ruta, cuerpo));

    public Task<RespuestaApi<object>> DeleteAsync(string ruta) =>
        EnviarAsync<object>(new HttpRequestMessage(HttpMethod.Delete, ruta));

    private static HttpRequestMessage ConCuerpo(HttpMethod metodo, string ruta, object cuerpo) =>
        new(metodo, ruta) { Content = JsonContent.Create(cuerpo, cuerpo.GetType(), options: Json) };

    private async Task<RespuestaApi<T>> EnviarAsync<T>(HttpRequestMessage solicitud)
    {
        try
        {
            using var respuesta = await http.SendAsync(solicitud);
            var texto = await respuesta.Content.ReadAsStringAsync();

            if (!respuesta.IsSuccessStatusCode)
            {
                return new RespuestaApi<T> { Codigo = respuesta.StatusCode, Mensaje = ExtraerMensaje(texto) };
            }

            T? datos = default;

            if (!string.IsNullOrWhiteSpace(texto) && typeof(T) != typeof(object))
            {
                datos = JsonSerializer.Deserialize<T>(texto, Json);
            }

            return new RespuestaApi<T> { Codigo = respuesta.StatusCode, Datos = datos };
        }
        catch (HttpRequestException)
        {
            return new RespuestaApi<T>
            {
                Codigo = HttpStatusCode.ServiceUnavailable,
                Mensaje = "No fue posible conectar con el servicio. Intente de nuevo."
            };
        }
    }

    private static string? ExtraerMensaje(string texto)
    {
        try
        {
            using var documento = JsonDocument.Parse(texto);

            if (documento.RootElement.ValueKind == JsonValueKind.Object &&
                documento.RootElement.TryGetProperty("mensaje", out var mensaje))
            {
                return mensaje.GetString();
            }
        }
        catch (JsonException)
        {
        }

        return null;
    }
}
