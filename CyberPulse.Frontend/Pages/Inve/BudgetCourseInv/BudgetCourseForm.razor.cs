using CurrieTechnologies.Razor.SweetAlert2;
using CyberPulse.Frontend.Respositories;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Threading.Tasks;

namespace CyberPulse.Frontend.Pages.Inve.BudgetCourseInv;

public partial class BudgetCourseForm
{
    private EditContext editContext = null!;

    private BudgetProgram2DTO selectedBudgetProgram = new();
    private List<BudgetProgram2DTO>? budgetPrograms;

    private Lot2DTO selectedLot = new();
    private List<Lot2DTO>? lots;
                                 
    private CourseProgramLot1DTO selectedCourse = new();
    private List<CourseProgramLot1DTO>? courseProgramLot;

    private bool _worthHasError = false;
    private string _worthErrorMessage = string.Empty;
    private bool loading;
    private bool _disable;
    
    private DateTime minDate = DateTime.Today.AddDays(0);

    [EditorRequired, Parameter] public BudgetCourseDTO BudgetCourseDTO { get; set; } = null!;
    [EditorRequired, Parameter] public EventCallback OnValidSubmit { get; set; }
    [EditorRequired, Parameter] public EventCallback ReturnAction { get; set; }

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;


    public bool FormPostedSuccessfully { get; set; } = false;


    protected override void OnInitialized()
    {
        editContext = new(BudgetCourseDTO);
    }
    protected override async Task OnInitializedAsync()
    {
        loading = true;

        await LoadBudgetProgramAsync();

        //if (BudgetCourseDTO.Id > 0)
        //{
        //    selectedBudgetProgram = budgetPrograms!.FirstOrDefault(x => x.Id == BudgetCourseDTO.CourseProgramLot!.ProgramLotId)!;
            
        //    await LoadLotsAsync(selectedBudgetProgram.ProgramId);

        //    selectedLot = lots!.FirstOrDefault(x => x.Id == BudgetCourseDTO.CourseProgramLot!.ProgramLot!.LotId)!;
        //    BudgetCourseDTO.BudgetLot = selectedLot;


        //    await LoadCoursesAsync(BudgetCourseDTO.CourseProgramLot!.ProgramLotId);


        //}
        //else
        //{
        //    selectedValidity = validities!.FirstOrDefault(x => x.IsInvalid = true)!;
        //    BudgetCourseDTO.ValidityId = selectedValidity.Id;
        //    BudgetCourseDTO.Validity = selectedValidity;

        //    BudgetCourseDTO.Validity = selectedValidity;
        //    BudgetCourseDTO.BudgetTypeId = 1;
        //}

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

    private async Task LoadBudgetProgramAsync()
    {
        var responseHttp = await Repository.GetAsync<List<BudgetProgram2DTO>>("/api/budgetprograms/combo");

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            return;
        }

        budgetPrograms = responseHttp.Response;
    }
    private async Task<IEnumerable<BudgetProgram2DTO>> SearchBudgetProgram(string searchText, CancellationToken cancellationToken)
    {
        await Task.Delay(5);

        if (string.IsNullOrWhiteSpace(searchText))
        {
            return budgetPrograms!;
        }

        return budgetPrograms!
            .Where(x => x.Program!.Name.ToString().Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }
    private async Task BudgetProgramChanged(BudgetProgram2DTO entity)
    {
        selectedBudgetProgram = entity;
        BudgetCourseDTO.BudgetProgram = entity;
        selectedLot = new();
        selectedCourse = new();
        await LoadLotsAsync(entity.ProgramId);
    }

    private async Task LoadLotsAsync(int id)
    {
        var responseHttp = await Repository.GetAsync<List<Lot2DTO>>($"/api/lots/combo/{id}");

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            return;
        }

        lots = responseHttp.Response;
    }
    private async Task<IEnumerable<Lot2DTO>> SearchLot(string searchText, CancellationToken cancellationToken)
    {
        await Task.Delay(5);

        if (string.IsNullOrWhiteSpace(searchText))
        {
            return lots!;
        }

        return lots!
            .Where(x => x.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }
    private async Task LotChanged(Lot2DTO entity)
    {
        selectedLot = entity;        
        BudgetCourseDTO.BudgetLot = entity;
        selectedCourse = new();
        await LoadCoursesAsync(entity.Id);
    }

    private async Task LoadCoursesAsync(int id)
    {
        var responseHttp = await Repository.GetAsync<List<CourseProgramLot1DTO>>($"/api/courseprogramlots/combo/{id}");

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            return;
        }

        courseProgramLot = responseHttp.Response;
    }
    private async Task<IEnumerable<CourseProgramLot1DTO>> SearchCourses(string searchText, CancellationToken cancellationToken)
    {
        await Task.Delay(5);

        if (string.IsNullOrWhiteSpace(searchText))
        {
            return courseProgramLot!;
        }

        return courseProgramLot!
            .Where(x => x.Course!.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase) ||
                        x.Course!.Code.ToString().Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }
    private void CoursesChanged(CourseProgramLot1DTO entity)
    {
        selectedCourse = entity;        
        BudgetCourseDTO.CourseProgramLot = entity;
        BudgetCourseDTO.CourseProgramLotId = entity.Id;
    }

    private void ValidateWorth()
    {
        _worthHasError = false;

        _worthErrorMessage = string.Empty;

        if (selectedLot.Name ==null)
        {
            BudgetCourseDTO.Worth = 0;
            Snackbar.Add(Localizer["RequiredLot"], Severity.Error);
        }
        else if(BudgetCourseDTO.Worth > selectedLot.Worth)
        {
            _worthHasError = true;
            _worthErrorMessage = Localizer["WorthExceedsLotBudget"];
        }
        else if (BudgetCourseDTO.Worth <= 0)
        {
            _worthHasError = true;
            _worthErrorMessage = Localizer["WorthMustBeGreaterThanZero"];
        }
    }
}