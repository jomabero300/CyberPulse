using CyberPulse.Frontend.Respositories;
using CyberPulse.Shared.EntitiesDTO.Chipp;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace CyberPulse.Frontend.Pages.Chipp;

public partial class ChipEdit
{
    private ChipForm? chipForm;

    private ChipDTO? chipDTO;
    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private ISqlInjValRepository _sqlValidator { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Parameter] public int Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var responseHttp = await Repository.GetAsync<ChipDTO>($"/api/chips/{Id}");

        if (responseHttp.Error)
        {
            if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                NavigationManager.NavigateTo("/chips");
            }
            else
            {
                var messageError = await responseHttp.GetErrorMessageAsync();

                Snackbar.Add(Localizer[messageError!], Severity.Error);
            }
        }
        else
        {
            chipDTO = responseHttp.Response;
        }

    }
    private async Task EditAsync()
    {
        if (_sqlValidator.HasSqlInjection(chipDTO!.ChipNo) ||
            _sqlValidator.HasSqlInjection(chipDTO!.Company) ||
            _sqlValidator.HasSqlInjection(chipDTO!.InstructorId) ||
            _sqlValidator.HasSqlInjection(chipDTO!.Justification))
        {
            Snackbar.Add(Localizer["ERR010"], Severity.Error);
            return;
        }


        if (chipDTO!.EndDate <= DateTime.Parse("01/01/2009"))
        {
            Snackbar.Add(Localizer["EndDateError"], Severity.Error);
            return;
        }

        if (chipDTO.StatuId == 7) chipDTO.StatuId += 1;

        chipDTO.idEsta = true;

        var responseHttp = await Repository.PutAsync("api/chips/full/", chipDTO);

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
        chipForm!.FormPostedSuccessfully = true;

        NavigationManager.NavigateTo("/chips");
    }
}