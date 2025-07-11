using CyberPulse.Shared.Entities.Gene;
using System.Net.Http.Json;
namespace CyberPulse.Frontend.Services;

public class AlertaService
{
    private readonly HttpClient _http;

    public AlertaService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Alerta>> VerificarAlertas()
    {
        var response = await _http.PostAsJsonAsync("api/alertas/verificar", "");
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<List<Alerta>>();

        return new List<Alerta>();
    }
}
