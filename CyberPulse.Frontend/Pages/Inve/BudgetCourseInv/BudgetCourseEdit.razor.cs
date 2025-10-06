using CyberPulse.Frontend.Respositories;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace CyberPulse.Frontend.Pages.Inve.BudgetCourseInv;

public partial class BudgetCourseEdit
{
    private BudgetCourseForm? BudgetCourseForm;

    private BudgetCourseDTO? BudgetCourseDTO;

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private ISqlInjValRepository _sqlValidator { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Parameter] public int Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var responseHttp = await Repository.GetAsync<BudgetCourseDTO>($"/api/budgetcourses/{Id}");

        if (responseHttp.Error)
        {
            if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                NavigationManager.NavigateTo("/budgetcourses");
            }
            else
            {
                var messageError = await responseHttp.GetErrorMessageAsync();

                Snackbar.Add(Localizer[messageError!], Severity.Error);
            }
        }
        else
        {
            BudgetCourseDTO = responseHttp.Response;
        }
    }

    private async Task EditAsync()
    {

        if (_sqlValidator.HasSqlInjection(BudgetCourseDTO!.StartDate.ToString()!) ||
            _sqlValidator.HasSqlInjection(BudgetCourseDTO!.EndDate.ToString()!) ||
            _sqlValidator.HasSqlInjection(BudgetCourseDTO!.Worth.ToString()))
        {
            //Datos del formulario no válidos
            Snackbar.Add(Localizer["ERR010"], Severity.Error);
            return;
        }

        var responseHttp = await Repository.PutAsync("api/budgetcourses/full", BudgetCourseDTO);

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
        BudgetCourseForm!.FormPostedSuccessfully = true;

        NavigationManager.NavigateTo("/budgetcourses");
    }
}