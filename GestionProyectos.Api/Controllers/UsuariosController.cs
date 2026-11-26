using System.Text.RegularExpressions;
using GestionProyectos.Api.Data;
using GestionProyectos.Api.Data.Entidades;
using GestionProyectos.Api.Dtos;
using GestionProyectos.Api.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionProyectos.Api.Controllers;

[Route("api/usuarios")]
public partial class UsuariosController(GestionDbContext db, IPasswordHasher<Usuario> hasher) : ApiControllerBase
{
    private const string MensajeContrasena =
        "La contraseña debe tener al menos 8 caracteres, con una mayúscula, una minúscula y un número.";

    [GeneratedRegex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$")]
    private static partial Regex ReglaContrasena();

    [HttpGet]
    public async Task<ActionResult<List<UsuarioDto>>> Listar()
    {
        var consulta = db.Usuarios.AsNoTracking();

        if (!User.EsAdministrador())
        {
            consulta = consulta.Where(u => u.Activo);
        }

        return await consulta
            .OrderBy(u => u.Nombre)
            .Select(u => new UsuarioDto
            {
                Id = u.Id,
                Nombre = u.Nombre,
                Email = u.Email,
                IdRol = u.IdRol,
                Rol = u.Rol.Nombre,
                FechaRegistro = u.FechaRegistro,
                Activo = u.Activo
            })
            .ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UsuarioDto>> Obtener(int id)
    {
        var usuario = await db.Usuarios
            .AsNoTracking()
            .Where(u => u.Id == id)
            .Select(u => new UsuarioDto
            {
                Id = u.Id,
                Nombre = u.Nombre,
                Email = u.Email,
                IdRol = u.IdRol,
                Rol = u.Rol.Nombre,
                FechaRegistro = u.FechaRegistro,
                Activo = u.Activo
            })
            .FirstOrDefaultAsync();

        if (usuario is null)
        {
            return NoEncontrado("El usuario no existe.");
        }

        return usuario;
    }

    [HttpPost]
    [Authorize(Roles = ClaimsPrincipalExtensiones.RolAdministrador)]
    public async Task<ActionResult<UsuarioDto>> Crear(UsuarioGuardarDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Contrasena) || !ReglaContrasena().IsMatch(dto.Contrasena))
        {
            return Peticion(MensajeContrasena);
        }

        if (!await db.Roles.AnyAsync(r => r.Id == dto.IdRol))
        {
            return Peticion("El rol seleccionado no es válido.");
        }

        if (await db.Usuarios.AnyAsync(u => u.Email == dto.Email))
        {
            return Conflicto("Ya existe un usuario con ese correo.");
        }

        var usuario = new Usuario
        {
            Nombre = dto.Nombre.Trim(),
            Email = dto.Email.Trim(),
            IdRol = dto.IdRol,
            Activo = dto.Activo
        };
        usuario.ContrasenaHash = hasher.HashPassword(usuario, dto.Contrasena);

        db.Usuarios.Add(usuario);
        await db.SaveChangesAsync();

        var resultado = new UsuarioDto
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Email = usuario.Email,
            IdRol = usuario.IdRol,
            Rol = await db.Roles.Where(r => r.Id == usuario.IdRol).Select(r => r.Nombre).FirstAsync(),
            FechaRegistro = usuario.FechaRegistro,
            Activo = usuario.Activo
        };

        return CreatedAtAction(nameof(Obtener), new { id = usuario.Id }, resultado);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = ClaimsPrincipalExtensiones.RolAdministrador)]
    public async Task<IActionResult> Actualizar(int id, UsuarioGuardarDto dto)
    {
        var usuario = await db.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

        if (usuario is null)
        {
            return NoEncontrado("El usuario no existe.");
        }

        if (!string.IsNullOrWhiteSpace(dto.Contrasena) && !ReglaContrasena().IsMatch(dto.Contrasena))
        {
            return Peticion(MensajeContrasena);
        }

        if (!await db.Roles.AnyAsync(r => r.Id == dto.IdRol))
        {
            return Peticion("El rol seleccionado no es válido.");
        }

        if (await db.Usuarios.AnyAsync(u => u.Email == dto.Email && u.Id != id))
        {
            return Conflicto("Ya existe un usuario con ese correo.");
        }

        usuario.Nombre = dto.Nombre.Trim();
        usuario.Email = dto.Email.Trim();
        usuario.IdRol = dto.IdRol;
        usuario.Activo = dto.Activo;

        if (!string.IsNullOrWhiteSpace(dto.Contrasena))
        {
            usuario.ContrasenaHash = hasher.HashPassword(usuario, dto.Contrasena);
        }

        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = ClaimsPrincipalExtensiones.RolAdministrador)]
    public async Task<IActionResult> Desactivar(int id)
    {
        var usuario = await db.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

        if (usuario is null)
        {
            return NoEncontrado("El usuario no existe.");
        }

        usuario.Activo = false;
        await db.SaveChangesAsync();
        return NoContent();
    }
}
