using CyberPulse.Frontend.Respositories;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace CyberPulse.Frontend.Pages.Inve.UnitMeasurementInv;

public partial class UnitMeasurementEdit
{
    private UnitMeasurementForm? unitmeasurementForm;

    private UnitMeasurementDTO? unitMeasurementDTO;


    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private ISqlInjValRepository _sqlValidator { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Parameter] public int Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var responseHttp = await Repository.GetAsync<UnitMeasurementDTO>($"/api/unitmeasurements/{Id}");

        if (responseHttp.Error)
        {
            if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                NavigationManager.NavigateTo("/unitmeasurements");
            }
            else
            {
                var messageError = await responseHttp.GetErrorMessageAsync();

                Snackbar.Add(Localizer[messageError!], Severity.Error);
            }
        }
        else
        {
            unitMeasurementDTO = responseHttp.Response;
        }
    }

    private async Task EditAsync()
    {
        if (_sqlValidator.HasSqlInjection(unitMeasurementDTO!.Name) ||
            _sqlValidator.HasSqlInjection(unitMeasurementDTO!.Symbol))
        {
            Snackbar.Add(Localizer["ERR010"], Severity.Error);
            return;
        }

        var responseHttp = await Repository.PutAsync("api/unitmeasurements/full", unitMeasurementDTO);

        if (responseHttp.Error)
        {
            var messageError = await responseHttp.GetErrorMessageAsync();

            Snackbar.Add(Localizer[messageError!], Severity.Error);

            return;
        }
        Return();

        Snackbar.Add(Localizer["RecordSavedOk"], Severity.Success);

    }

    private void Return()
    {
        unitmeasurementForm!.FormPostedSuccessfully = true;

        NavigationManager.NavigateTo("/unitmeasurements");
    }

}