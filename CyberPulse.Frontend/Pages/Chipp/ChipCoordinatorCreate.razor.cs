using CurrieTechnologies.Razor.SweetAlert2;
using CyberPulse.Frontend.Respositories;
using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.EntitiesDTO.Chipp;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Security.Claims;

namespace CyberPulse.Frontend.Pages.Chipp;

[Authorize(Roles = "Coor")]
public partial class ChipCoordinatorCreate
{
    private ChipCoordinatorForm? chipCoordinatorForm;
    private ChipCoordinator chipCoordinator = new();
    private ChipDTO chipDTO = new();

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private ISqlInjValRepository _sqlValidator { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Inject] private IRepository repository { get; set; } = null!;

    private async Task CreateAsync()
    {

        if (_sqlValidator.HasSqlInjection(chipCoordinator!.Identificacion) ||
            _sqlValidator.HasSqlInjection(chipCoordinator!.Code) ||
            _sqlValidator.HasSqlInjection(chipCoordinator!.ChipNo))
        {
            Snackbar.Add(Localizer["ERR010"], Severity.Error);
            return;
        }



        chipDTO.ChipNo = string.IsNullOrWhiteSpace(chipCoordinator.ChipNo) ? "0000": chipCoordinator.ChipNo;
        chipDTO.ChipProgramId = chipCoordinator.ChipProgramId;
        chipDTO.InstructorId = chipCoordinator.InstructorId;
        chipDTO.StartDate = chipCoordinator.StartDate ?? DateTime.Now;
        chipDTO.EndDate = DateTime.Parse("1999/01/01");
        chipDTO.AlertDate = DateTime.Parse("1999/01/01");
        chipDTO.NeighborhoodId = 81001000;
        chipDTO.TrainingProgramId = 1;
        chipDTO.TypeOfTrainingId = 1;
        chipDTO.Justification = ".";
        chipDTO.Apprentices = 1;
        chipDTO.Company = "";
        chipDTO.MondayMorningStart = TimeSpan.Zero;
        chipDTO.MondayMorningEnd = TimeSpan.Zero;
        chipDTO.MondayAfternoonEnd = TimeSpan.Zero;
        chipDTO.MondayAfternoonStart = TimeSpan.Zero;

        chipDTO.TuesdayMorningStart = TimeSpan.Zero;
        chipDTO.TuesdayMorningEnd = TimeSpan.Zero;
        chipDTO.TuesdayAfternoonEnd = TimeSpan.Zero;
        chipDTO.TuesdayAfternoonStart = TimeSpan.Zero;

        chipDTO.WednesdayMorningStart = TimeSpan.Zero;
        chipDTO.WednesdayMorningEnd = TimeSpan.Zero;
        chipDTO.WednesdayAfternoonEnd = TimeSpan.Zero;
        chipDTO.WednesdayAfternoonStart = TimeSpan.Zero;

        chipDTO.ThursdayMorningStart = TimeSpan.Zero;
        chipDTO.ThursdayMorningEnd = TimeSpan.Zero;
        chipDTO.ThursdayAfternoonEnd = TimeSpan.Zero;
        chipDTO.ThursdayAfternoonStart = TimeSpan.Zero;

        chipDTO.FridayMorningStart = TimeSpan.Zero;
        chipDTO.FridayMorningEnd = TimeSpan.Zero;
        chipDTO.FridayAfternoonEnd = TimeSpan.Zero;
        chipDTO.FridayAfternoonStart = TimeSpan.Zero;

        chipDTO.SaturdayMorningStart = TimeSpan.Zero;
        chipDTO.SaturdayMorningEnd = TimeSpan.Zero;
        chipDTO.SaturdayAfternoonEnd = TimeSpan.Zero;
        chipDTO.SaturdayAfternoonStart = TimeSpan.Zero;

        chipDTO.SundayMorningStart = TimeSpan.Zero;
        chipDTO.SundayMorningEnd = TimeSpan.Zero;
        chipDTO.SundayAfternoonEnd = TimeSpan.Zero;
        chipDTO.SundayAfternoonStart = TimeSpan.Zero;
        chipDTO.UserId = await UserSearchAsync();
        chipDTO.StatuId = await SearchIndEstaAsync("Creada", 1);
        chipDTO.idEsta = true;

        chipDTO.City = new() { Id = 81005, Name = "Arauca" };



        chipDTO.ChipProgram = new()
        {
            Id = chipCoordinator.ChipProgramId,
            Code = chipCoordinator.Code,
            Designation = "asdasd",
            Duration = chipCoordinator.StatuId,
            StartDate = new DateTime(),
            WingMeasure = true,
        };
        chipDTO.Instructor = new()
        {
            Id = chipCoordinator.InstructorId,
            DocumentId = "252222",
            FirstName = chipCoordinator.InstructorName,
            LastName = chipCoordinator.InstructorName,
        };
        chipDTO.TypeOfTraining = new()
        {
            Id = 1,
            Name = "Parla"
        };
        chipDTO.TrainingProgram = new()
        {
            Id = 1,
            Name = "Perlas"
        };

        chipDTO.TypeOfPoblationDTO = new();
        chipDTO.language = System.Globalization.CultureInfo.CurrentCulture.Name.Substring(0, 2);

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

    private async Task<string> UserSearchAsync()
    {
        var responseHttp = await Repository.GetAsync<User>("/api/accounts");

        return responseHttp.Response!.Id;
    }
    private async Task<int> SearchIndEstaAsync(string name, int nivel)
    {
        var responseHttp = await repository.GetAsync<Statu>($"/api/status/full?name={name}&nivel={nivel}");

        if (responseHttp.Error)
        {
            return 0;
        }

        return responseHttp.Response!.Id;
    }

    private void Return()
    {
        chipCoordinatorForm!.FormPostedSuccessfully = true;
        NavigationManager.NavigateTo("/chips");
    }
}