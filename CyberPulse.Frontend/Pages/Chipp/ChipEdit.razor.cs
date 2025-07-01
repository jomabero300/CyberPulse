using CyberPulse.Frontend.Respositories;
using CyberPulse.Shared.EntitiesDTO.Chipp;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace CyberPulse.Frontend.Pages.Chipp;

public partial class ChipEdit
{
    private ChipForm? chipForm;

    private ChipDTO? chipDTO;
    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Parameter] public int Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var responseHttp = await Repository.GetAsync<ChipDTO>($"/api/chips/{Id}");

        if (responseHttp.Error)
        {
            if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                NavigationManager.NavigateTo("/chips");
            }
            else
            {
                var messageError = await responseHttp.GetErrorMessageAsync();

                Snackbar.Add(Localizer[messageError!], Severity.Error);
            }
        }
        else
        {
            chipDTO = responseHttp.Response;
        }

    }
    private async Task EditAsync()
    {
        string menssaje = "";

        if (chipDTO!.ChipProgramId == 0)
        {

            menssaje = "SelectAChipProgram";
        }
        else if (string.IsNullOrWhiteSpace(chipDTO.InstructorId))
        {
            menssaje = "SelectAInstructor";
        }
        else if (chipDTO.NeighborhoodId == 0)
        {
            menssaje = "SelectACity";
        }
        else if (chipDTO.TrainingProgramId == 0)
        {
            menssaje = "TrainingProgram";
        }
        else if (chipDTO.TrainingProgramId == 1 && chipDTO.TypeOfTrainingId == 0)
        {
            menssaje = "SelectATypeOfTraining";

        }
        else if (!chipDTO.WingMeasure && string.IsNullOrWhiteSpace(chipDTO.Company))
        {
            menssaje = "Company";
        }
        else if (chipDTO.EndDate <= DateTime.Parse("01/01/2009"))
        {
            menssaje = "EndDateError";
        }

        if (menssaje != "")
        {
            Snackbar.Add(Localizer[menssaje!], Severity.Error);
            return;
        }



        var responseHttp = await Repository.PutAsync("api/chips/full/", chipDTO);

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
        chipForm!.FormPostedSuccessfully = true;

        NavigationManager.NavigateTo("/chips");
    }
}