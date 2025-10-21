using CyberPulse.Frontend.Respositories;
using CyberPulse.Shared.EntitiesDTO.Chipp;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace CyberPulse.Frontend.Pages.Chipp;

[Authorize(Roles = "Coor")]
public partial class ChipCoordinatorEdit
{
    private ChipCoordinatorForm? chipCoordinatorForm;
    private ChipCoordinator? chipCoordinator;
    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private ISqlInjValRepository _sqlValidator { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Parameter] public int Id { get; set; }
    protected override async Task OnInitializedAsync()
    {
        var responseHttp = await Repository.GetAsync<ChipCoordinator>($"/api/chips/full?Id={Id}&indEsta=False");

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
            chipCoordinator = responseHttp.Response;
        }
    }
    private async Task EditAsync()
    {
        if (_sqlValidator.HasSqlInjection(chipCoordinator!.Identificacion) ||
            _sqlValidator.HasSqlInjection(chipCoordinator!.Code) ||
            _sqlValidator.HasSqlInjection(chipCoordinator!.ChipNo))
        {
            Snackbar.Add(Localizer["ERR010"], Severity.Error);
            return;
        }

        chipCoordinator.language = System.Globalization.CultureInfo.CurrentCulture.Name.Substring(0, 2);
        var responseHttp = await Repository.PutAsync("api/chips/fullc/", chipCoordinator);

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
        chipCoordinatorForm!.FormPostedSuccessfully = true;

        NavigationManager.NavigateTo("/chips");
    }
}