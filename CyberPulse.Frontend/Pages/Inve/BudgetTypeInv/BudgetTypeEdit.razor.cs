using CyberPulse.Frontend.Respositories;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace CyberPulse.Frontend.Pages.Inve.BudgetTypeInv;

public partial class BudgetTypeEdit
{
    private BudgetTypeForm? budgetTypeForm;

    private BudgetType? BudgetType;


    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private ISqlInjValRepository _sqlValidator { get; set; } = null!;

    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Parameter] public int Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var responseHttp = await Repository.GetAsync<BudgetType>($"/api/budgettypes/{Id}");

        if (responseHttp.Error)
        {
            if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                NavigationManager.NavigateTo("/budgettypes");
            }
            else
            {
                var messageError = await responseHttp.GetErrorMessageAsync();

                Snackbar.Add(Localizer[messageError!], Severity.Error);
            }
        }
        else
        {
            BudgetType = responseHttp.Response;
        }
    }

    private async Task EditAsync()
    {
        // Validar contra SQL injection
        if (_sqlValidator.HasSqlInjection(BudgetType!.Name))
        {
            //Datos del formulario no válidos
            Snackbar.Add(Localizer["ERR010"], Severity.Error);
            return;
        }
        var responseHttp = await Repository.PutAsync("api/budgettypes/full", BudgetType);

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
        budgetTypeForm!.FormPostedSuccessfully = true;

        NavigationManager.NavigateTo("/budgettypes");
    }

}