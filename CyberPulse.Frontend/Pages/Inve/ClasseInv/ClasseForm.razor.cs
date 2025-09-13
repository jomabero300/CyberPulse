using CurrieTechnologies.Razor.SweetAlert2;
using CyberPulse.Frontend.Respositories;
using CyberPulse.Shared.EntitiesDTO.Gene;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace CyberPulse.Frontend.Pages.Inve.ClasseInv;

public partial class ClasseForm
{
    private EditContext editContext = null!;

    private Family2DTO selectedFamily = new();
    private List<Family2DTO>? families;

    private SegmentDTO selectedSegment = new();
    private List<SegmentDTO>? segments;

    private StatuDTO selectedStatu = new();
    private List<StatuDTO>? status;

    private bool loading;
    private bool _disable;

    [EditorRequired, Parameter] public ClasseDTO ClasseDTO { get; set; } = null!;
    [EditorRequired, Parameter] public EventCallback OnValidSubmit { get; set; }
    [EditorRequired, Parameter] public EventCallback ReturnAction { get; set; }

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;


    public bool FormPostedSuccessfully { get; set; } = false;
    protected override void OnInitialized()
    {
        editContext = new(ClasseDTO);
    }

    protected override async Task OnInitializedAsync()
    {
        loading = true;

        await LoadStatusAsync();
        await LoadSegmentsAsync();

        if (ClasseDTO.Id > 0)
        {
            await LoadFamilyAsync(ClasseDTO.Family!.SegmentId);

            selectedStatu = status!.FirstOrDefault(x => x.Id == ClasseDTO.StatuId)!;

            selectedFamily = families!.FirstOrDefault(x => x.Id == ClasseDTO.FamilyId)!;
            ClasseDTO.Family=selectedFamily;




            selectedSegment = segments!.FirstOrDefault(x => x.Id == selectedFamily.SegmentId)!;
            ClasseDTO.Segment = selectedSegment;
        }
        else
        {
            _disable = true;
            selectedStatu = status!.FirstOrDefault(x => x.Name == "Activo")!;
        }

        ClasseDTO.Statu = selectedStatu;
        ClasseDTO.StatuId = selectedStatu.Id;

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
    private async Task SegmentChanged(SegmentDTO entity)
    {
        selectedSegment = entity;
        await LoadFamilyAsync(entity.Id);
        ClasseDTO.Segment = entity;
    }

    private async Task LoadFamilyAsync(int id)
    {
        var responseHttp = await Repository.GetAsync<List<Family2DTO>>($"/api/families/combo/{id}");

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            return;
        }

        families = responseHttp.Response;

    }
    private async Task<IEnumerable<Family2DTO>> SearchFamily(string searchText, CancellationToken cancellationToken)
    {
        await Task.Delay(5);
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return families!;
        }

        return families!
            .Where(x => x.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }
    private void FamilyChanged(Family2DTO entity)
    {
        selectedFamily = entity;
        ClasseDTO.Family = entity;
        ClasseDTO.FamilyId = entity.Id;
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
        ClasseDTO.StatuId = entity.Id;
        ClasseDTO.Statu=entity;
    }
}