using CurrieTechnologies.Razor.SweetAlert2;
using CyberPulse.Frontend.Respositories;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Localization;

namespace CyberPulse.Frontend.Pages.Inve.CourseProgramLotInv;

public partial class CourseProgramLotForm
{
    private EditContext editContext = null!;

    private InvProgramDTO selectedProgram = new();
    private List<InvProgramDTO>? programs;

    private ProgramLot2DTO selectedLot = new();
    private List<ProgramLot2DTO>? lots;

    private CourseDTO selectedCourse = new();
    private List<CourseDTO>? courses;

    private bool _disable;
    private bool loading;

    [EditorRequired, Parameter] public CourseProgramLotDTO CourseProgramLotDTO { get; set; } = null!;
    [EditorRequired, Parameter] public EventCallback OnValidSubmit { get; set; }
    [EditorRequired, Parameter] public EventCallback ReturnAction { get; set; }

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;


    public bool FormPostedSuccessfully { get; set; } = false;
    protected override void OnInitialized()
    {
        editContext = new(CourseProgramLotDTO);
    }
    protected override async Task OnInitializedAsync()
    {
        loading = true;

        await LoadProgramAsync();

        if (CourseProgramLotDTO.Id > 0)
        {
            selectedProgram = programs!.FirstOrDefault(x => x.Id == CourseProgramLotDTO!.ProgramLot!.ProgramId)!;
            CourseProgramLotDTO.ProgramLot = selectedLot;

            await LoadLotsAsync(selectedProgram.Id);
            selectedLot = lots!.FirstOrDefault(x => x.Id == CourseProgramLotDTO!.ProgramLotId)!;
            CourseProgramLotDTO.ProgramLot = selectedLot;

            await LoadCoursesAsync(CourseProgramLotDTO.Id);
            selectedCourse = courses!.FirstOrDefault(x => x.Id == CourseProgramLotDTO!.CourseId)!;
            CourseProgramLotDTO.Course = selectedCourse;
        }

        loading = false;
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

    private async Task LoadProgramAsync()
    {
        var responseHttp = await Repository.GetAsync<List<InvProgramDTO>>("/api/invprograms/combo");

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            return;
        }

        programs = responseHttp.Response;
    }
    private async Task<IEnumerable<InvProgramDTO>> SearchProgram(string searchText, CancellationToken cancellationToken)
    {
        await Task.Delay(5);
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return programs!;
        }

        return programs!
            .Where(x => x.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }
    private async Task ProgramChanged(InvProgramDTO entity)
    {
        selectedProgram = entity;
        selectedLot = new();
        selectedCourse = new();
        await LoadLotsAsync(entity.Id);
    }

    private async Task LoadLotsAsync(int id)
    {
        var responseHttp = await Repository.GetAsync<List<ProgramLot2DTO>>($"/api/programlots/combo/{id}");

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            return;
        }

        lots = responseHttp.Response;
    }
    private async Task<IEnumerable<ProgramLot2DTO>> SearchLot(string searchText, CancellationToken cancellationToken)
    {
        await Task.Delay(5);

        if (string.IsNullOrWhiteSpace(searchText))
        {
            return lots!;
        }

        return lots!
            .Where(x => x.Lot!.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }
    private async Task LotChanged(ProgramLot2DTO entity)
    {
        selectedLot = entity;
        CourseProgramLotDTO.ProgramLotId = entity.Id;
        CourseProgramLotDTO.ProgramLot = entity;
        selectedCourse = new();
        await LoadCoursesAsync(entity.Id);
    }


    private async Task LoadCoursesAsync(int id)
    {
        var responseHttp = await Repository.GetAsync<List<CourseDTO>>($"/api/courses/combo/{id}/{(CourseProgramLotDTO.Id > 0)}");

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            return;
        }

        courses = responseHttp.Response;
    }
    private async Task<IEnumerable<CourseDTO>> SearchCourse(string searchText, CancellationToken cancellationToken)
    {
        await Task.Delay(5);

        if (string.IsNullOrWhiteSpace(searchText))
        {
            return courses!;
        }

        return courses!
            .Where(x => x.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }
    private void CourseChanged(CourseDTO entity)
    {
        selectedCourse = entity;
        CourseProgramLotDTO.CourseId = entity.Id;
        CourseProgramLotDTO.Course = entity;
    }

}