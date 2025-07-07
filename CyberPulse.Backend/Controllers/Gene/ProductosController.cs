using CyberPulse.Backend.Helpers;
using CyberPulse.Shared.Entities.Gene;
using Microsoft.AspNetCore.Mvc;

namespace CyberPulse.Backend.Controllers.Gene;

[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private static readonly List<Producto> productos = new()
    {
        new Producto { Id = 1, Nombre = "Laptop", Cantidad = 5 },
        new Producto { Id = 2, Nombre = "Mouse", Cantidad = 10 },
        new Producto { Id = 3, Nombre = "Teclado", Cantidad = 7 }
    };

    [HttpGet]
    public IEnumerable<Producto> Get() => productos;

    [HttpGet("reporte")]
    public IActionResult GenerarReporte()
    {
        var pdf = ReporteService.GenerarPdf(productos);

        return File(pdf, "application/pdf", "reporte_productos.pdf");
    }
}