using CyberPulse.Frontend.Respositories;
using CyberPulse.Shared.Entities.Chipp;
using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.EntitiesDTO.Chipp.Report;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using MudBlazor;

namespace CyberPulse.Frontend.Pages.Chipp;

[Authorize(Roles = "Admi,Coor")]
public partial class ChipReports
{
    private ChipReport chipReport = new();
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Inject] private IRepository repository { get; set; } = null!;
    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IDialogService DialogService { get; set; } = null!;
    [Inject] IJSRuntime JS { get; set; } = null!;

    [Parameter] public int Id { get; set; }
    private async Task SearchInstructor()
    {
        if (string.IsNullOrWhiteSpace(chipReport!.Identificacion))
        {
            chipReport.InstructorId = "";
            chipReport.InstructorName = "";
            chipReport.Identificacion = "";
            return;
        }

        chipReport.InstructorId = "";
        chipReport.InstructorName = "";
        var responseHttp = await repository.GetAsync<User>($"/api/accounts/Instructor?Id={chipReport.Identificacion}&userType=inst");
        chipReport.Identificacion = "";

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(Localizer[message!], Severity.Error);
            return;
        }

        chipReport.Identificacion = responseHttp.Response!.DocumentId;
        chipReport.InstructorId = responseHttp.Response.Id;
        chipReport.InstructorName = responseHttp.Response.FullName;
    }
    private async Task UserDialogo()
    {
        var parameters = new DialogParameters();
        var dialog = await DialogService.ShowAsync<ChipCoordinatorSearchInstructor>("Buscar Persona", parameters);
        var result = await dialog.Result;

        if (!result.Canceled && result.Data is User userDTO)
        {
            chipReport!.InstructorId = userDTO.Id;
            chipReport.Identificacion = userDTO.DocumentId;
            chipReport.InstructorName = $"{userDTO.FirstName} {userDTO.LastName}";
        }
    }
    private async Task SearchProgram()
    {
        if (string.IsNullOrWhiteSpace(chipReport.Code))
        {
            chipReport.Code = "";
            chipReport.ChipProgramId = 0;
            chipReport.ChipProgramName = "";
            return;
        }

        chipReport.ChipProgramName = "";
        chipReport.ChipProgramId = 0;

        var responseHttp = await repository.GetAsync<ChipProgram>($"/api/ChipPrograms/Program/{chipReport.Code}");
        chipReport.Code = "";

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(Localizer[message!], Severity.Error);
            return;
        }
        chipReport.Code = responseHttp.Response!.Code;
        chipReport.ChipProgramId = responseHttp.Response!.Id;
        chipReport.ChipProgramName = responseHttp.Response.Designation;
    }
    private async Task ProgramDialogo()
    {
        var parameters = new DialogParameters();
        var dialog = await DialogService.ShowAsync<ChipCoordinatorSearchPrograms>("Buscar programa", parameters);
        var result = await dialog.Result;

        if (!result.Canceled && result.Data is ChipProgram programDTO)
        {
            chipReport.Code = programDTO.Code;
            chipReport.ChipProgramId = programDTO.Id;
            chipReport.ChipProgramName = programDTO.Designation;
        }

    }
    private void Return()
    {
        NavigationManager.NavigateTo("/");
    }

    private async Task ReportPdf()
    {
        //string filter = "api/chips/report?";
        //if(!string.IsNullOrEmpty(chipReport.ChipNo))
        //{
        //    filter += $"ChipNo={chipReport.ChipNo}&";
        //}
        //if(!string.IsNullOrEmpty(chipReport.Identificacion))
        //{
        //    filter += $"InstructorId={chipReport.InstructorId}&";
        //}
        //if(!string.IsNullOrEmpty(chipReport.Code))
        //{
        //    filter += $"ChipProgramId={chipReport.ChipProgramId} &";
        //}
        //if (chipReport.StartDate != null)
        //{
        //    filter += $"StartDate={chipReport.StartDate}&";
        //}
        //if (chipReport.EndDate!= null)
        //{
        //    filter += $"EndDate={chipReport.EndDate}&";
        //}
        //if (chipReport.AlertDate != null)
        //{
        //    filter += $"AlertDate ={chipReport.AlertDate}&";
        //}

        //if (filter == "?")
        //{
        //    filter = "";
        //}
        //else
        //{
        //    filter=filter.Substring(0, filter.Length-1);
        //}

        var response = await repository.GetBytesAsync("api/chips/report/",chipReport);

        if (response.Error || response.Response == null)
        {
            // Handle error
            return;
        }

        await JS.InvokeVoidAsync("displayPdf", response.Response);

    }
}