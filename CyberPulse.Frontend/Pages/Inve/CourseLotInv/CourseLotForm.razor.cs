using CurrieTechnologies.Razor.SweetAlert2;
using CyberPulse.Frontend.Respositories;
using CyberPulse.Shared.EntitiesDTO.Gene;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Localization;

namespace CyberPulse.Frontend.Pages.Inve.CourseLotInv;

public partial class CourseLotForm
{
    //private EditContext editContext = null!;

    //private CourseDTO selectedCourse = new();
    //private List<CourseDTO>? courses;

    //private LotDTO selectedLot = new();
    //private List<LotDTO>? lots;


    //private bool loading;

    //[EditorRequired, Parameter] public CourseLotDTO CourseLotDTO { get; set; } = null!;
    //[EditorRequired, Parameter] public EventCallback OnValidSubmit { get; set; }
    //[EditorRequired, Parameter] public EventCallback ReturnAction { get; set; }

    //[Inject] private IRepository Repository { get; set; } = null!;
    //[Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
    //[Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;


    //public bool FormPostedSuccessfully { get; set; } = false;
    //protected override void OnInitialized()
    //{
    //    editContext = new(CourseLotDTO);
    //}

    //protected override async Task OnInitializedAsync()
    //{
    //    loading = true;

    //    await LoadCoursesAsync();

    //    if (CourseLotDTO.Id > 0)
    //    {
    //        await LoadLotsAsync(CourseLotDTO.CourseId);

    //        selectedLot = lots!.FirstOrDefault(x => x.Id == CourseLotDTO.ProgramLot!.LotId)!;
    //    }

    //    loading = false;
    //}

    //private async Task LoadLotsAsync(int id)
    //{
    //    var responseHttp = await Repository.GetAsync<List<LotDTO>>($"/api/lots/combo/{id}");

    //    if (responseHttp.Error)
    //    {
    //        var message = await responseHttp.GetErrorMessageAsync();
    //        await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
    //        return;
    //    }

    //    lots = responseHttp.Response;
    //}
    //private async Task<IEnumerable<SegmentDTO>> SearchLot(string searchText, CancellationToken cancellationToken)
    //{
    //    await Task.Delay(5);
    //    if (string.IsNullOrWhiteSpace(searchText))
    //    {
    //        return segments!;
    //    }

    //    return segments!
    //        .Where(x => x.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
    //        .ToList();
    //}
    //private async Task LotChanged(SegmentDTO entity)
    //{
    //    selectedSegment = entity;
    //    await LoadFamilyAsync(entity.Id);
    //    CourseLotDTO.Segment = entity;
    //}


    //private async Task OnBeforeInternalNavigation(LocationChangingContext context)
    //{
    //    var formwasEditad = editContext.IsModified();

    //    if (!formwasEditad || FormPostedSuccessfully)
    //    {
    //        return;
    //    }

    //    var result = await SweetAlertService.FireAsync(new SweetAlertOptions
    //    {
    //        Title = Localizer["Confirmation"],
    //        Text = Localizer["LeaveAndLoseChanges"],
    //        Icon = SweetAlertIcon.Warning,
    //        ShowCancelButton = true,
    //    });

    //    var confirm = !string.IsNullOrEmpty(result.Value);

    //    if (confirm)
    //    {
    //        return;
    //    }

    //    context.PreventNavigation();
    //}

    //private async Task LoadCoursesAsync()
    //{
    //    var responseHttp = await Repository.GetAsync<List<StatuDTO>>("/api/status/combo/0");

    //    if (responseHttp.Error)
    //    {
    //        var message = await responseHttp.GetErrorMessageAsync();
    //        await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
    //        return;
    //    }

    //    status = responseHttp.Response;
    //}
    //private async Task<IEnumerable<StatuDTO>> SearchStatu(string searchText, CancellationToken cancellationToken)
    //{
    //    await Task.Delay(5);
    //    if (string.IsNullOrWhiteSpace(searchText))
    //    {
    //        return status!;
    //    }

    //    return status!
    //        .Where(x => x.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
    //        .ToList();
    //}
    //private void StatuChanged(StatuDTO entity)
    //{
    //    selectedStatu = entity;
    //    CourseLotDTO.StatuId = entity.Id;
    //    CourseLotDTO.Statu = entity;
    //}
}