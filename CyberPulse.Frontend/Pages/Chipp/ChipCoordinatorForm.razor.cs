using CurrieTechnologies.Razor.SweetAlert2;
using CyberPulse.Frontend.Respositories;
using CyberPulse.Shared.Entities.Chipp;
using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.EntitiesDTO.Chipp;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Security.Claims;

namespace CyberPulse.Frontend.Pages.Chipp;

public partial class ChipCoordinatorForm
{
    private EditContext editContext = null!;
    [EditorRequired, Parameter] public ChipCoordinator chipCoordinator { get; set; } = null!;
    [EditorRequired, Parameter] public EventCallback OnValidSubmit { get; set; }
    [EditorRequired, Parameter] public EventCallback ReturnAction { get; set; }
    [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    [Inject] private IRepository repository { get; set; } = null!;

    public bool FormPostedSuccessfully { get; set; } = false;
    private bool DisabledTypeOfTraining = true;
    private bool loading;
    private DateTime minDate = DateTime.Today.AddDays(-5);
    protected override void OnInitialized()
    {
        editContext = new(chipCoordinator);

    }


    private async Task OnBeforeInternalNavigation(LocationChangingContext context)
    {
        var formwasEditad = editContext.IsModified();

        if (!formwasEditad || FormPostedSuccessfully)
        {
            return;
        }

        var result = await SweetAlertService.FireAsync(new SweetAlertOptions
        {
            Title = Localizer["Confirmation"],
            Text = Localizer["LeaveAndLoseChanges"],
            Icon = SweetAlertIcon.Warning,
            ShowCancelButton = true,
        });

        var confirm = !string.IsNullOrEmpty(result.Value);

        if (confirm)
        {
            return;
        }

        context.PreventNavigation();
    }

    private async Task SearchInstructor()
    {
        if (string.IsNullOrWhiteSpace(chipCoordinator.Identificacion))
            return;

        chipCoordinator.InstructorId = "";
        chipCoordinator.InstructorName = "";
        var responseHttp = await repository.GetAsync<User>($"/api/accounts/Instructor?Id={chipCoordinator.Identificacion}&userType=inst");
        chipCoordinator.Identificacion = "";

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(Localizer[message!], Severity.Error);
            return;
        }

        chipCoordinator.Identificacion = responseHttp.Response!.DocumentId;
        chipCoordinator.InstructorId = responseHttp.Response.Id;
        chipCoordinator.InstructorName = responseHttp.Response.FullName;
    }

    private async Task SearchProgram()
    {
        if (string.IsNullOrWhiteSpace(chipCoordinator.Code))
            return;

        chipCoordinator.ChipProgramName = "";
        chipCoordinator.ChipProgramId = 0;

        var responseHttp = await repository.GetAsync<ChipProgram>($"/api/ChipPrograms/Program/{chipCoordinator.Code}");
        chipCoordinator.Code = "";

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(Localizer[message!], Severity.Error);
            return;
        }
        chipCoordinator.Code = responseHttp.Response!.Code;
        chipCoordinator.ChipProgramId = responseHttp.Response!.Id;
        chipCoordinator.ChipProgramName = responseHttp.Response.Designation;
    }
}