using CurrieTechnologies.Razor.SweetAlert2;
using CyberPulse.Frontend.Pages.Chipp;
using CyberPulse.Frontend.Respositories;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO.Chipp.Report;
using CyberPulse.Shared.EntitiesDTO.Gene;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using MudBlazor;

namespace CyberPulse.Frontend.Pages.Inve.BudgetInv;

public partial class BudgetReport
{
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Inject] private IRepository repository { get; set; } = null!;
    [Inject] private ISqlInjValRepository _sqlValidator { get; set; } = null!;
    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IDialogService DialogService { get; set; } = null!;
    [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
    [Inject] IJSRuntime JS { get; set; } = null!;

    private Validity selectedValidity = new();
    private List<Validity>? validities;

    protected override async Task OnInitializedAsync()
    {
        await LoadValidityAsync();
    }

    private async Task LoadValidityAsync()
    {
        var responseHttp = await repository.GetAsync<List<Validity>>("/api/Validities/combo");

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            return;
        }

        validities = responseHttp.Response;
    }
    private async Task<IEnumerable<Validity>> SearchValidityAsync(string searchText, CancellationToken cancellationToken)
    {
        await Task.Delay(5);

        if (string.IsNullOrWhiteSpace(searchText))
        {
            return validities!;
        }

        return validities!
            .Where(x => x.Value.ToString().Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }
    private void ValidityChanged(Validity entity)
    {
        selectedValidity = entity;
    }


    private async Task ReportPdf()
    {
        var response = await repository.GetBytesAsync($"api/budgetcourses/report/{selectedValidity.Id}");

        if (response.Error || response.Response == null)
        {
            // Handle error
            return;
        }

        await JS.InvokeVoidAsync("displayPdf", response.Response);
    }
}