using CyberPulse.Frontend.Respositories;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace CyberPulse.Frontend.Pages.Inve.PurchasingBoardInv;

public partial class PurchasingBoardEdit
{
    private PurchasingBoardForm? purchasingBoardForm;

    private ProductQuotationPurcDTO? productQuotationPurcDTO;

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private ISqlInjValRepository _sqlValidator { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Parameter] public ProductQuotationPurcDTO ProductQuatation { get; set; } = null!;

    protected override void OnInitialized()
    {
        productQuotationPurcDTO = ProductQuatation;
    }

    private async Task EditAsync()
    {
        if (productQuotationPurcDTO!.Estado) return;

        if (_sqlValidator.HasSqlInjection(productQuotationPurcDTO!.Quoted01.ToString()) ||
            _sqlValidator.HasSqlInjection(productQuotationPurcDTO!.Quoted02.ToString()) ||
            _sqlValidator.HasSqlInjection(productQuotationPurcDTO!.Quoted03.ToString()))
        {
            Snackbar.Add(Localizer["ERR010"], Severity.Error);
            return;
        }

        var responseHttp = await Repository.PutAsync("api/productquotations/fulls", productQuotationPurcDTO);

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
        purchasingBoardForm!.FormPostedSuccessfully = true;

        NavigationManager.NavigateTo("/purchasingboard");
    }
}