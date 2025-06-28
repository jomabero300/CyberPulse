using CyberPulse.Frontend.Respositories;
using CyberPulse.Shared.EntitiesDTO.Chipp;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace CyberPulse.Frontend.Pages.Chipp;

public partial class ChipCreate
{
    private ChipForm? chipForm;
    private ChipDTO? chipDTO = new();
    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    private async Task CreateAsync()
    {
        if (chipDTO!.ChipProgramId == 0)
        {
            var message = "SelectAChipProgram";
            Snackbar.Add(Localizer[message!], Severity.Error);
            return;
        }

        var responseHttp = await Repository.PostAsync("/api/chips/full", chipDTO);

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(Localizer[message!], Severity.Error);
            return;
        }

        Return();
        Snackbar.Add(Localizer["RecordCreateOk"], Severity.Success);
    }
    private void Return()
    {
        chipForm!.FormPostedSuccessfully = true;
        NavigationManager.NavigateTo("/chips");
    }

}