using GestionProyectos.Api.Data;
using GestionProyectos.Api.Data.Entidades;
using GestionProyectos.Api.Dtos;
using GestionProyectos.Api.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionProyectos.Api.Controllers;

[Route("api/auth")]
public class AuthController(GestionDbContext db, IPasswordHasher<Usuario> hasher, TokenService tokens) : ApiControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest solicitud)
    {
        var usuario = await db.Usuarios
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.Email == solicitud.Email && u.Activo);

        if (usuario is null ||
            hasher.VerifyHashedPassword(usuario, usuario.ContrasenaHash, solicitud.Contrasena) == PasswordVerificationResult.Failed)
        {
            return StatusCode(StatusCodes.Status401Unauthorized, new { mensaje = "Correo o contraseña incorrectos" });
        }

        var (token, expira) = tokens.Crear(usuario, usuario.Rol.Nombre);
        return new LoginResponse { Token = token, Expira = expira };
    }

    [HttpGet("me")]
    public async Task<ActionResult<MeResponse>> Me()
    {
        var idUsuario = User.IdUsuario();

        var usuario = await db.Usuarios
            .AsNoTracking()
            .Where(u => u.Id == idUsuario && u.Activo)
            .Select(u => new MeResponse { Id = u.Id, Nombre = u.Nombre, Email = u.Email, Rol = u.Rol.Nombre })
            .FirstOrDefaultAsync();

        if (usuario is null)
        {
            return StatusCode(StatusCodes.Status401Unauthorized, new { mensaje = "La sesión ya no es válida." });
        }

        return usuario;
    }
}
