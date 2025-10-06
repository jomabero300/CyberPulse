using CurrieTechnologies.Razor.SweetAlert2;
using CyberPulse.Frontend.Respositories;
using CyberPulse.Shared.EntitiesDTO.Gene;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Xml.Linq;

namespace CyberPulse.Frontend.Pages.Inve.BudgetInv;

public partial class BudgetForm
{
    private EditContext editContext = null!;

    private ValidityDTO selectedValidity = new();
    private List<ValidityDTO>? validities;

    private bool loading;
    private bool _disable;

    [EditorRequired, Parameter] public BudgetDTO BudgetDTO { get; set; } = null!;
    [EditorRequired, Parameter] public EventCallback OnValidSubmit { get; set; }
    [EditorRequired, Parameter] public EventCallback ReturnAction { get; set; }

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;


    public bool FormPostedSuccessfully { get; set; } = false;
    protected override void OnInitialized()
    {
        editContext = new(BudgetDTO);
    }

    protected override async Task OnInitializedAsync()
    {
        loading = true;

        await LoadValiditiesAsync();

        if (BudgetDTO.Id > 0)
        {
            selectedValidity = validities!.FirstOrDefault(x => x.Id == BudgetDTO.ValidityId)!;
            BudgetDTO.Validity = selectedValidity;
        }
        else
        {
            if(validities == null || !validities.Any())
            {
                Snackbar.Add(Localizer["ERR014"], Severity.Error);
                return;
            }

            selectedValidity = validities!.FirstOrDefault(x => x.IsInvalid=true)!;
            BudgetDTO.ValidityId = selectedValidity.Id;
            BudgetDTO.Validity = selectedValidity;            

            BudgetDTO.Validity = selectedValidity;
            BudgetDTO.BudgetTypeId = 1;
            BudgetDTO.StatuId = 1;
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

    private async Task LoadValiditiesAsync()
    {
        var responseHttp = await Repository.GetAsync<List<ValidityDTO>>("/api/validities/combo");

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            return;
        }

        validities = responseHttp.Response;
    }
    private async Task<IEnumerable<ValidityDTO>> SearchValidity(string searchText, CancellationToken cancellationToken)
    {
        await Task.Delay(5);
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return validities!;
        }

        return validities!
            .Where(x => x.Value.ToString().Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }
    private void ValidityChanged(ValidityDTO entity)
    {
        selectedValidity = entity;
        BudgetDTO.Validity = entity;
        BudgetDTO.ValidityId = entity.Id;
    }
}