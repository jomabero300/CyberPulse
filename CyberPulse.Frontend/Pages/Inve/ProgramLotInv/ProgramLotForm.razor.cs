using CurrieTechnologies.Razor.SweetAlert2;
using CyberPulse.Frontend.Pages.Chipp;
using CyberPulse.Frontend.Respositories;
using CyberPulse.Shared.Entities.Chipp;
using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.EntitiesDTO.Chipp;
using CyberPulse.Shared.EntitiesDTO.Gene;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace CyberPulse.Frontend.Pages.Inve.ProgramLotInv;

public partial class ProgramLotForm
{
    private EditContext editContext = null!;

    private InvProgramDTO selectedProgram= new();
    private List<InvProgramDTO>? programs;

    private Lot2DTO selectedLot= new();
    private List<Lot2DTO>? lots;

    [EditorRequired, Parameter] public ProgramLotDTO ProgramLotDTO { get; set; } = null!;
    [EditorRequired, Parameter] public EventCallback OnValidSubmit { get; set; }
    [EditorRequired, Parameter] public EventCallback ReturnAction { get; set; }
    [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IRepository repository { get; set; } = null!;
    [Inject] private IDialogService DialogService { get; set; } = null!;

    public bool FormPostedSuccessfully { get; set; } = false;

    private bool loading;
    protected override void OnInitialized()
    {
        editContext = new(ProgramLotDTO);
    }
    protected override async Task OnInitializedAsync()
    {
        await LoadProgramsAsync();
        if(ProgramLotDTO.Id>0)
        {
            selectedProgram = programs!.FirstOrDefault(x => x.Id == ProgramLotDTO!.ProgramId)!;
            ProgramLotDTO.Program=selectedProgram;
            await LoadLotsAsync(ProgramLotDTO!.Id);

            selectedLot = lots!.FirstOrDefault(x => x.Id == ProgramLotDTO.LotId)!;
            ProgramLotDTO.Lot=selectedLot;
        }
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

    private async Task LoadProgramsAsync()
    {
        //var responseHttp = await repository.GetAsync<List<InvProgramDTO>>($"/api/InvPrograms/ProgramLot/{id}");


        var responseHttp = await repository.GetAsync<List<InvProgramDTO>>("/api/InvPrograms/combo");

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
        ProgramLotDTO.ProgramId = entity.Id;
        ProgramLotDTO.Program = entity;
        selectedLot = new();
        await LoadLotsAsync(entity.Id);
    }

    private async Task LoadLotsAsync(int id)
    {
        selectedLot = new();

        var responseHttp = await repository.GetAsync<List<Lot2DTO>>($"/api/lots/Combo/{id}/{(ProgramLotDTO.Id > 0)}");

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
    private void LotChanged(Lot2DTO entity)
    {
        selectedLot = entity;
        ProgramLotDTO.LotId = entity.Id;
        ProgramLotDTO.Lot = entity;
    }
}