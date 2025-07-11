using CyberPulse.Backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class AlertasController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AlertasController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("verificar")]
    public async Task<IActionResult> VerificarYEnviarAlertas()
    {
        var hoy = DateTime.Today;

        var alertas = await _context.Alertas
            .Where(a => a.FechaAlerta.Date == hoy.Date && !a.EstadoEnviado)
            .ToListAsync();

        foreach (var alerta in alertas)
        {
            // Simulación de envío de email
            await EnviarCorreo(alerta.Email, "Alerta de vencimiento", $"La alerta con fecha {alerta.FechaAlerta:dd-MM-yyyy} se ha activado.");
            alerta.EstadoEnviado = true;
        }

        await _context.SaveChangesAsync();

        return Ok(alertas);
    }

    private async Task EnviarCorreo(string para, string asunto, string cuerpo)
    {
        // Simulación - reemplazar por implementación real de SMTP o servicio externo
        await Task.Delay(500);
        Console.WriteLine($"Correo enviado a: {para}, Asunto: {asunto}");
    }
}
