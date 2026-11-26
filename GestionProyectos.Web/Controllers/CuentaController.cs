using System.Net;
using GestionProyectos.Web.Models;
using GestionProyectos.Web.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionProyectos.Web.Controllers;

public class CuentaController(AuthApi auth) : BaseController
{
    [AllowAnonymous]
    [HttpGet]
    public IActionResult Login()
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SesionKeys.Token)))
        {
            return RedirectToAction("Index", "Dashboard");
        }

        return View(new LoginViewModel());
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel modelo)
    {
        if (!ModelState.IsValid)
        {
            return View(modelo);
        }

        var login = await auth.LoginAsync(modelo.Email, modelo.Contrasena);

        if (!login.Exito)
        {
            ModelState.AddModelError("", login.Codigo == HttpStatusCode.Unauthorized
                ? login.Mensaje ?? "Correo o contraseña incorrectos"
                : login.Mensaje ?? "No fue posible iniciar sesión. Intente de nuevo.");
            return View(modelo);
        }

        HttpContext.Session.SetString(SesionKeys.Token, login.Datos!.Token);

        var yo = await auth.MeAsync();

        if (!yo.Exito)
        {
            HttpContext.Session.Clear();
            ModelState.AddModelError("", yo.Mensaje ?? "No fue posible iniciar sesión. Intente de nuevo.");
            return View(modelo);
        }

        HttpContext.Session.SetString(SesionKeys.IdUsuario, yo.Datos!.Id.ToString());
        HttpContext.Session.SetString(SesionKeys.Nombre, yo.Datos.Nombre);
        HttpContext.Session.SetString(SesionKeys.Rol, yo.Datos.Rol);

        return RedirectToAction("Index", "Dashboard");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction(nameof(Login));
    }
}
