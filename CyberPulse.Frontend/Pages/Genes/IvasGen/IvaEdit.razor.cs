using CyberPulse.Frontend.Pages.Genes.Status;
using CyberPulse.Frontend.Respositories;
using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.EntitiesDTO.Gene;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace CyberPulse.Frontend.Pages.Genes.IvasGen;

public partial class IvaEdit
{
    private IvaForm? IvaForm;
    private IvaFormDTO? IvaDTO;

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private ISqlInjValRepository _sqlValidator { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Parameter] public int Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var responseHttp = await Repository.GetAsync<IvaFormDTO>($"/api/ivas/{Id}");

        if (responseHttp.Error)
        {
            if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                NavigationManager.NavigateTo("/ivas");
            }
            else
            {
                var messageError = await responseHttp.GetErrorMessageAsync();

                Snackbar.Add(Localizer[messageError!], Severity.Error);
            }
        }
        else
        {
            IvaDTO = responseHttp.Response;
        }
    }

    private async Task EditAsync()
    {
        if (_sqlValidator.HasSqlInjection(IvaDTO!.Name))
        {
            Snackbar.Add(Localizer["ERR010"], Severity.Error);
            return;
        }
        var responseHttp = await Repository.PutAsync("api/ivas", IvaDTO);

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
        IvaForm!.FormPostedSuccessfully = true;

        NavigationManager.NavigateTo("/ivas");
    }
}