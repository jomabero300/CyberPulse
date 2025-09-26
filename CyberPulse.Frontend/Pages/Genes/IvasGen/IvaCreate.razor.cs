using CyberPulse.Frontend.Pages.Genes.Status;
using CyberPulse.Frontend.Respositories;
using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.EntitiesDTO.Gene;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace CyberPulse.Frontend.Pages.Genes.IvasGen;

public partial class IvaCreate
{
    private IvaForm? IvaForm;
    private IvaFormDTO IvaDTO = new();

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private ISqlInjValRepository _sqlValidator { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;

    private async Task CreateAsync()
    {
        if (_sqlValidator.HasSqlInjection(IvaDTO!.Name) ||
            _sqlValidator.HasSqlInjection(IvaDTO!.Worth.ToString()))
        {
            Snackbar.Add(Localizer["ERR010"], Severity.Error);
            return;
        }
        var responseHttp = await Repository.PostAsync("/api/ivas/full", IvaDTO);

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
        IvaForm!.FormPostedSuccessfully = true;
        NavigationManager.NavigateTo("/ivas");
    }
}