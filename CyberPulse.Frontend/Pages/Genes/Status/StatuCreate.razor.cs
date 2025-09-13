using CyberPulse.Frontend.Respositories;
using CyberPulse.Shared.Entities.Chipp;
using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace CyberPulse.Frontend.Pages.Genes.Status;

public partial class StatuCreate
{
    private StatuForm? statuForm;
    private Statu statu = new();

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private ISqlInjValRepository _sqlValidator { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;

    private async Task CreateAsync()
    {
        if (_sqlValidator.HasSqlInjection(statu!.Name))
        {
            Snackbar.Add(Localizer["ERR010"], Severity.Error);
            return;
        }
        var responseHttp = await Repository.PostAsync("/api/status", statu);

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
        statuForm!.FormPostedSuccessfully = true;
        NavigationManager.NavigateTo("/status");
    }
}