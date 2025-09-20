using CyberPulse.Frontend.Pages.Inve.ClasseInv;
using CyberPulse.Frontend.Respositories;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace CyberPulse.Frontend.Pages.Inve.BudgetInv;

public partial class BudgetCreate
{
    private BudgetForm? budgetForm;
    private BudgetDTO budgetDTO = new();

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private ISqlInjValRepository _sqlValidator { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;

    private async Task CreateAsync()
    {
        // Validar contra SQL injection
        if (_sqlValidator.HasSqlInjection(budgetDTO.Rubro)||
            _sqlValidator.HasSqlInjection(budgetDTO.Worth.ToString()))
        {
            //Datos del formulario no válidos
            Snackbar.Add(Localizer["ERR010"], Severity.Error);
            return;
        }


        var responseHttp = await Repository.PostAsync("/api/budgets/full", budgetDTO);

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
        budgetForm!.FormPostedSuccessfully = true;
        NavigationManager.NavigateTo("/budgets");
    }
}