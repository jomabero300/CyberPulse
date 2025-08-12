using CyberPulse.Frontend.Respositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace CyberPulse.Frontend.Pages.Genes.Reports;

[Authorize(Roles = "Admi")]
public partial class Productos
{
    [Inject] private IRepository repository { get; set; } = null!;

    [Inject] IJSRuntime JS { get; set; } = null!;
    private async Task MostrarReporte()
    {

        var response = await repository.GetBytesAsync("api/chips/Report");

        if (response.Error || response.Response == null)
        {
            // Handle error
            return;
        }

        await JS.InvokeVoidAsync("displayPdf", response.Response);
    }
}