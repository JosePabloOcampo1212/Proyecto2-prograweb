using GestionProyectos.Web.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace GestionProyectos.Web.Controllers;

public class DashboardController(DashboardApi dashboard) : BaseController
{
    public async Task<IActionResult> Index()
    {
        var indicadores = await dashboard.IndicadoresAsync();

        if (!indicadores.Exito)
        {
            return ErrorServicio(indicadores);
        }

        return View(indicadores.Datos);
    }
}
