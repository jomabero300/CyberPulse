using CyberPulse.Frontend.Pages.Inve.BudgetInv;
using CyberPulse.Frontend.Respositories;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace CyberPulse.Frontend.Pages.Inve.BudgetLotInv;

public partial class BudgetLotCreate
{
    private BudgetLotForm? budgetLotForm;
    private BudgetLotDTO budgetLotDTO = new();

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private ISqlInjValRepository _sqlValidator { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;

    private async Task CreateAsync()
    {
        //if (budgetLotDTO.Indesta == true) return;

        //if (_sqlValidator.HasSqlInjection(budgetLotDTO.Worth.ToString()))
        //{
        //    Snackbar.Add(Localizer["ERR010"], Severity.Error);
        //    return;
        //}


        var responseHttp = await Repository.PostAsync("/api/budgetlots/full", budgetLotDTO);

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
        budgetLotForm!.FormPostedSuccessfully = true;
        NavigationManager.NavigateTo("/budgetlots");
    }

}