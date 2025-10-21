using CyberPulse.Frontend.Respositories;
using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.EntitiesDTO.Chipp;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace CyberPulse.Frontend.Pages.Chipp;

public partial class ChipCreate
{
    private ChipForm? chipForm;
    private ChipDTO chipDTO = new();
    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private ISqlInjValRepository _sqlValidator { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Inject] private IRepository repository { get; set; } = null!;
    private async Task CreateAsync()
    {
        if (_sqlValidator.HasSqlInjection(chipDTO!.ChipNo) ||
            _sqlValidator.HasSqlInjection(chipDTO!.Company) ||
            _sqlValidator.HasSqlInjection(chipDTO!.InstructorId)||
            _sqlValidator.HasSqlInjection(chipDTO!.Justification))
        {
            Snackbar.Add(Localizer["ERR010"], Severity.Error);
            return;
        }

        if (chipDTO.EndDate <= DateTime.Parse("01/01/2009"))
        {
            Snackbar.Add(Localizer["EndDateError"], Severity.Error);
            return;
        }


        chipDTO.UserId = await userSearchAsync();

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

    private async Task<string> userSearchAsync()
    {
        var responseHttp = await repository.GetAsync<User>($"/api/accounts");

        return responseHttp!.Response!.Id;
    }

    private void Return()
    {
        chipForm!.FormPostedSuccessfully = true;
        NavigationManager.NavigateTo("/chips");
    }

}