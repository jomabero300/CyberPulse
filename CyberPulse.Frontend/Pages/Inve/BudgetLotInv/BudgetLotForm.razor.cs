using CurrieTechnologies.Razor.SweetAlert2;
using CyberPulse.Frontend.Respositories;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace CyberPulse.Frontend.Pages.Inve.BudgetLotInv;

public partial class BudgetLotForm
{
    private EditContext editContext = null!;
    [EditorRequired, Parameter] public BudgetLotDTO BudgetLotDTO { get; set; } = null!;
    [EditorRequired, Parameter] public EventCallback OnValidSubmit { get; set; }
    [EditorRequired, Parameter] public EventCallback ReturnAction { get; set; }

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;


    private BudgetProgram2DTO selectedBudgetProgram = new();
    private List<BudgetProgram2DTO>? budgetPrograms;

    private ProgramLot2DTO selectedLot = new();
    private List<ProgramLot2DTO>? lots;

    private bool loading;

    private bool _worthHasError = false;
    private string _worthErrorMessage = string.Empty;
    private bool _disable;



    public bool FormPostedSuccessfully { get; set; } = false;
    protected override void OnInitialized()
    {
        editContext = new(BudgetLotDTO);
    }
    protected override async Task OnInitializedAsync()
    {
        loading = true;
        BudgetLotDTO.StatuId = 1;
        await LoadBudgetProgramAsync();

        if (BudgetLotDTO.Id > 0)
        {
            selectedBudgetProgram = budgetPrograms!.FirstOrDefault(x => x.Id == BudgetLotDTO.BudgetProgramId)!;
            BudgetLotDTO.BudgetProgram = selectedBudgetProgram;
            await LoadLotsAsync(selectedBudgetProgram.ProgramId);

            selectedLot = lots!.FirstOrDefault(x => x.Id == BudgetLotDTO.ProgramLotId)!;
            BudgetLotDTO.ProgramLot = selectedLot;
            _disable = true;
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
        BudgetLotDTO.BudgetProgramId = entity.Id;
        BudgetLotDTO.BudgetProgram = entity;
        selectedLot = new();
        BudgetLotDTO.Worth = 0;
        await LoadLotsAsync(entity.ProgramId);
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
    private void LotChanged(ProgramLot2DTO entity)
    {
        selectedLot = entity;
        BudgetLotDTO.ProgramLotId = entity.Id;
        BudgetLotDTO.ProgramLot = entity;
    }
}