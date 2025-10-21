using CyberPulse.Frontend.Respositories;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace CyberPulse.Frontend.Pages.Inve.Validityinv;

public partial class ValidityCreate
{
    private ValidityForm? ValidityForm;
    private ValidityDTO ValidityDTO = new();

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private ISqlInjValRepository _sqlValidator { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        var responseHttp = await Repository.GetAsync("/api/validities/Validez");

        if (responseHttp.Error)
        {

            var message = await responseHttp.GetErrorMessageAsync();
            if (message == "ERRO11") //Crear y dejar que el usuario lo modifique por que es la primera vez 
            {
                ValidityDTO.Value = DateTime.UtcNow.Year + 1;
                ValidityDTO.IsInvalid = false;
                ValidityDTO.StatuId = 1;
                return;
            }

            Snackbar.Add(Localizer[message!], Severity.Error);
            Return();
            return;
        }

        ValidityDTO.Value = (int)responseHttp.Response!;
        ValidityDTO.IsInvalid = true;
    }

    private async Task CreateAsync()
    {
        var responseHttp = await Repository.PostAsync("/api/validities/full", ValidityDTO);

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
        ValidityForm!.FormPostedSuccessfully = true;
        NavigationManager.NavigateTo("/validities");
    }

}