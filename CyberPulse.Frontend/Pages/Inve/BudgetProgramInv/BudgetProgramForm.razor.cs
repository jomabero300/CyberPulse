using CurrieTechnologies.Razor.SweetAlert2;
using CyberPulse.Frontend.Respositories;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO.Gene;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace CyberPulse.Frontend.Pages.Inve.BudgetProgramInv;

public partial class BudgetProgramForm
{
    private EditContext editContext = null!;

    private bool loading;
    [EditorRequired, Parameter] public BudgetProgram1DTO BudgetProgramDTO { get; set; } = null!;
    [EditorRequired, Parameter] public EventCallback OnValidSubmit { get; set; }
    [EditorRequired, Parameter] public EventCallback ReturnAction { get; set; }

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;


    private BudgetDTO? selectedBudget { get; set; }=null;
    private List<BudgetDTO>? budgets;

    private InvProgramDTO selectedProgram = new();
    private List<InvProgramDTO>? programs;

    private bool _worthHasError = false;
    private string _worthErrorMessage = string.Empty;
    public bool FormPostedSuccessfully { get; set; } = false;
    public bool _enabled { get; set; } = false;
    protected override void OnInitialized()
    {
        editContext = new(BudgetProgramDTO);
       
        BudgetProgramDTO.Budget = null;
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadBudgetsAsync();
        await LoadProgramsAsync();

        if (BudgetProgramDTO.Id > 0)
        {
            selectedBudget = budgets!.FirstOrDefault(x => x.Id == BudgetProgramDTO.BudgetId)!;
            BudgetProgramDTO.BudgetId = selectedBudget.Id;
            BudgetProgramDTO.Budget = selectedBudget;

            selectedProgram = programs!.FirstOrDefault(x => x.Id == BudgetProgramDTO.ProgramId)!;
            BudgetProgramDTO.ProgramId = selectedProgram.Id;
            BudgetProgramDTO.Program = selectedProgram;
            _enabled = true;
        }


        var responseHttp = await Repository.GetAsync<List<ValidityDTO>>("/api/validities/combo");

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            return;
        }
        //budgets = responseHttp.Response;

        BudgetProgramDTO.ValidityId = responseHttp.Response!.FirstOrDefault(x=>x.StatuId==1)!.Id;

        BudgetProgramDTO.StatuId = 1;

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

    private async Task LoadBudgetsAsync()
    {
        var responseHttp = await Repository.GetAsync<List<BudgetDTO>>("/api/budgets/combo");

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            return;
        }
        budgets = responseHttp.Response;
    }
    private async Task<IEnumerable<BudgetDTO>> SearchBudget(string searchText, CancellationToken cancellationToken)
    {
        await Task.Delay(5);
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return budgets!;
        }

        return budgets!
            .Where(x => x.Rubro.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }
    private void BudgetChanged(BudgetDTO entity)
    {
        selectedBudget = entity;
        BudgetProgramDTO.Budget = entity;
        BudgetProgramDTO.BudgetId = entity.Id;
    }


    private async Task LoadProgramsAsync()
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
    private void ProgramChanged(InvProgramDTO entity)
    {
        selectedProgram = entity;
        BudgetProgramDTO.Program= entity;
        BudgetProgramDTO.ProgramId = entity.Id;
        //TODO: VALIDAR SI ES INICIAL O ADICIONAL PARA GUARDAR
        BudgetProgramDTO.BudgetTypeId=entity.StatuId;
    }

    private async Task ValidateWorth()
    {
        BudgetProgramDTO.Indesta = false;
        _worthHasError = false;
        _worthErrorMessage = string.Empty;

        if (selectedBudget == null)
        {
            Snackbar.Add(Localizer["RequiredRubro"], Severity.Error);
            BudgetProgramDTO.Indesta = true;

        }else if (BudgetProgramDTO.Worth > selectedBudget!.Worth)
        {
            BudgetProgramDTO.Indesta = true;

            _worthHasError = true;
            _worthErrorMessage = $"{Localizer["BudgetProgramWorth"]} {selectedBudget!.Worth:N2}.";
        }
    }
}