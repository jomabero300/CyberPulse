using CyberPulse.Frontend.Respositories;
using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.EntitiesDTO.Chipp;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace CyberPulse.Frontend.Pages.Chipp;

public partial class ChipCreate
{
    private ChipForm? chipForm;
    private ChipDTO? chipDTO = new();
    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Inject] private IRepository repository { get; set; } = null!;
    private async Task CreateAsync()
    {
        string menssaje = "";

        if (chipDTO!.ChipProgramId == 0)
        {

            menssaje="SelectAChipProgram";            
        }
        else if(string.IsNullOrWhiteSpace(chipDTO.InstructorId))
        {
            menssaje = "SelectAInstructor";
        }
        else if(chipDTO.NeighborhoodId==0)
        {
            menssaje = "SelectACity";
        }
        else if(chipDTO.TrainingProgramId==0)
        {
            menssaje = "TrainingProgram";
        }
        else if(!chipDTO.WingMeasure && string.IsNullOrWhiteSpace(chipDTO.Company))
        {
            menssaje = "Company";
        }
        else if(chipDTO.EndDate<=DateTime.Parse("01/011/2029"))
        {
            menssaje = "EndDateError";
        }

        if (menssaje != "")
        {
            Snackbar.Add(Localizer[menssaje!], Severity.Error);
            return;
        }

        chipDTO.UserId=await userSearchAsync();

        var responseHttp = await Repository.PostAsync("/api/chips/full", chipDTO);

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(Localizer[message!], Severity.Error);
            return;
        }

        Return();
        Snackbar.Add(Localizer["RecordCreateOk"], Severity.Success);
    }

    private async Task<string> userSearchAsync()
    {
        var responseHttp = await repository.GetAsync<User>($"/api/accounts");

        return responseHttp!.Response!.Id;
    }

    private void Return()
    {
        chipForm!.FormPostedSuccessfully = true;
        NavigationManager.NavigateTo("/chips");
    }

}