using CyberPulse.Frontend.Respositories;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace CyberPulse.Frontend.Pages.Inve.ProductCurrentValueInv;

public partial class ProductCurrentValueAllPercentage
{
    private ProductCurrentValueAllPercentageForm? ProductCurrentValueForm;
    private ProductCurrentValueSpDTO ProductCurrentValueSpDTO = new();

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private ISqlInjValRepository _sqlValidator { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;

    private async Task CreateAsync()
    {
        if (_sqlValidator.HasSqlInjection(ProductCurrentValueSpDTO.Percentage.ToString()))
        {
            Snackbar.Add(Localizer["ERR010"], Severity.Error);
            return;
        }

        var ltcade = $"/api/productcurrentvalues/all/{ProductCurrentValueSpDTO.Percentage.ToString().Replace(',', '.')}";

        var responseHttp = await Repository.GetAsync(ltcade);

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
        ProductCurrentValueForm!.FormPostedSuccessfully = true;
        NavigationManager.NavigateTo("/productcurrentvalues");
    }
}