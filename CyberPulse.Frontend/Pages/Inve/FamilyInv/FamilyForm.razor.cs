using CurrieTechnologies.Razor.SweetAlert2;
using CyberPulse.Frontend.Respositories;
using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.EntitiesDTO.Gene;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Localization;

namespace CyberPulse.Frontend.Pages.Inve.FamilyInv;

public partial class FamilyForm
{
    private EditContext editContext = null!;

    private StatuDTO selectedStatu = new();
    private List<StatuDTO>? status;

    private SegmentDTO selectedSegment = new();
    private List<SegmentDTO>? segments;

    private bool loading;
    private bool _disable;
    
    [EditorRequired, Parameter] public FamilyDTO FamilyDTO { get; set; } = null!;
    [EditorRequired, Parameter] public EventCallback OnValidSubmit { get; set; }
    [EditorRequired, Parameter] public EventCallback ReturnAction { get; set; }

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    public bool FormPostedSuccessfully { get; set; } = false;
    protected override void OnInitialized()
    {
        editContext = new(FamilyDTO);
    }

    protected override async Task OnInitializedAsync()
    {
        loading = true;

        await LoadStatusAsync();
        await LoadSegmentsAsync();

        if (FamilyDTO.Id > 0)
        {
            selectedStatu = status!.FirstOrDefault(x => x.Id == FamilyDTO.StatuId)!;
            selectedSegment = segments!.FirstOrDefault(x => x.Id == FamilyDTO.SegmentId)!;
            FamilyDTO.Segment = selectedSegment;
        }
        else
        {
            _disable = true;
            selectedStatu = status!.FirstOrDefault(x => x.Name == "Activo")!;
            FamilyDTO.StatuId = selectedStatu.Id;
        }
        
        FamilyDTO.Statu = selectedStatu;

        loading = false;
    }

    private async Task LoadSegmentsAsync()
    {
        var responseHttp = await Repository.GetAsync<List<SegmentDTO>>("/api/segments/combo");

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            return;
        }

        segments = responseHttp.Response;
    }
    private async Task<IEnumerable<SegmentDTO>> SearchSegment(string searchText, CancellationToken cancellationToken)
    {
        await Task.Delay(5);
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return segments!;
        }

        return segments!
            .Where(x => x.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }
    private void SegmentChanged(SegmentDTO entity)
    {
        selectedSegment = entity;
        FamilyDTO.SegmentId= entity.Id;
        FamilyDTO.Segment = selectedSegment;
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

    private async Task LoadStatusAsync()
    {
        var responseHttp = await Repository.GetAsync<List<StatuDTO>>("/api/status/combo/0");

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            return;
        }

        status = responseHttp.Response;
    }
    private async Task<IEnumerable<StatuDTO>> SearchStatu(string searchText, CancellationToken cancellationToken)
    {
        await Task.Delay(5);
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return status!;
        }

        return status!
            .Where(x => x.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }
    private void StatuChanged(StatuDTO entity)
    {
        selectedStatu = entity;
        FamilyDTO.StatuId = entity.Id;
    }

}