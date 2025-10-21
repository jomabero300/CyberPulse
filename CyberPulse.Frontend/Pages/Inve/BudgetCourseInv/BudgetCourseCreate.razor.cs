using CyberPulse.Frontend.Respositories;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace CyberPulse.Frontend.Pages.Inve.BudgetCourseInv;

public partial class BudgetCourseCreate
{
    private BudgetCourseForm? budgetCourseForm;
    private BudgetCourseDTO budgetCourseDTO = new();

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private ISqlInjValRepository _sqlValidator { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;

    private async Task CreateAsync()
    {
        if (_sqlValidator.HasSqlInjection(budgetCourseDTO.StartDate.ToString()) ||
            _sqlValidator.HasSqlInjection(budgetCourseDTO.EndDate.ToString()) ||
            _sqlValidator.HasSqlInjection(budgetCourseDTO.Worth.ToString()))
        {
            //Datos del formulario no válidos
            Snackbar.Add(Localizer["ERR010"], Severity.Error);
            return;
        }

        budgetCourseDTO.ValidityId = 1;
        budgetCourseDTO.StatuId = 1;

        var responseHttp = await Repository.PostAsync("/api/budgetcourses/full", budgetCourseDTO);

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
        budgetCourseForm!.FormPostedSuccessfully = true;
        NavigationManager.NavigateTo("/budgetcourses");
    }
}