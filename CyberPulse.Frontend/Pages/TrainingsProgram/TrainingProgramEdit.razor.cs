using CyberPulse.Frontend.Respositories;
using CyberPulse.Shared.Entities.Chipp;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace CyberPulse.Frontend.Pages.TrainingsProgram;

public partial class TrainingProgramEdit
{
    private TrainingProgramForm? trainingProgramForm;

    private TrainingProgram? trainingProgram;
    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private ISqlInjValRepository _sqlValidator { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Parameter] public int Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var responseHttp = await Repository.GetAsync<TrainingProgram>($"/api/trainingprograms/{Id}");

        if (responseHttp.Error)
        {
            if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                NavigationManager.NavigateTo("/programs");
            }
            else
            {
                var messageError = await responseHttp.GetErrorMessageAsync();

                Snackbar.Add(Localizer[messageError!], Severity.Error);
            }
        }
        else
        {
            trainingProgram = responseHttp.Response;
        }
    }

    private async Task EditAsync()
    {
        if (_sqlValidator.HasSqlInjection(trainingProgram!.Name))
        {
            Snackbar.Add(Localizer["ERR010"], Severity.Error);
            return;
        }
        var responseHttp = await Repository.PutAsync("api/Trainingprograms", trainingProgram);

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
        trainingProgramForm!.FormPostedSuccessfully = true;

        NavigationManager.NavigateTo("/programs");
    }

}