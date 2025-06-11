using CyberPulse.Frontend.Respositories;
using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace CyberPulse.Frontend.Pages.Genes.Status;

public partial class StatuIndex
{
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Inject] private IRepository Repository { get; set; } = null!;

    private List<Statu>? Status { get; set; }
    protected override async Task OnInitializedAsync()
    {
        var responseHppt = await Repository.GetAsync<List<Statu>>("api/status");
        Status = responseHppt.Response!;
    }
}