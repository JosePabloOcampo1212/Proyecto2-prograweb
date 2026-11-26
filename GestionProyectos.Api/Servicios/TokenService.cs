using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GestionProyectos.Api.Data.Entidades;
using Microsoft.IdentityModel.Tokens;

namespace GestionProyectos.Api.Servicios;

public class TokenService(IConfiguration configuracion)
{
    public (string Token, DateTime Expira) Crear(Usuario usuario, string rol)
    {
        var minutos = configuracion.GetValue<int>("Jwt:MinutosExpiracion");
        var expira = DateTime.UtcNow.AddMinutes(minutos);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.Nombre),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Role, rol)
        };

        var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuracion["Jwt:Key"]!));
        var credenciales = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: configuracion["Jwt:Issuer"],
            audience: configuracion["Jwt:Audience"],
            claims: claims,
            expires: expira,
            signingCredentials: credenciales);

        return (new JwtSecurityTokenHandler().WriteToken(token), expira);
    }
}
