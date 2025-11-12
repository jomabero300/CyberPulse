using CurrieTechnologies.Razor.SweetAlert2;
using CyberPulse.Frontend.Respositories;
using CyberPulse.Shared.EntitiesDTO.Gene;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Resources;
using MailKit.Search;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace CyberPulse.Frontend.Pages.Inve.ProductInv;

public partial class ProductForm
{
    private EditContext editContext = null!;

    private SegmentDTO selectedSegment = new();
    private List<SegmentDTO>? segments;

    private FamilyDTO selectedFamily = new();
    private List<FamilyDTO>? families;

    private Classe2DTO selectedClasse = new();
    private List<Classe2DTO>? classes;

    private Lot2DTO selectedLot = new();
    private List<Lot2DTO>? lots;

    private UnitMeasurementDTO selectedUnitMeasurement = new();
    private List<UnitMeasurementDTO>? unitMeasurements;

    private StatuDTO selectedStatu = new();
    private List<StatuDTO>? status;

    private bool loading;
    private bool _disable = true;

    [EditorRequired, Parameter] public ProductFormDTO ProductDTO { get; set; } = null!;
    [EditorRequired, Parameter] public EventCallback OnValidSubmit { get; set; }
    [EditorRequired, Parameter] public EventCallback ReturnAction { get; set; }

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;


    public bool FormPostedSuccessfully { get; set; } = false;
    protected override void OnInitialized()
    {
        editContext = new(ProductDTO);
    }

    protected override async Task OnInitializedAsync()
    {
        loading = true;

        await LoadSegmentAsync();
        await LoadUnitMeasurementAsync();
        await LoadStatusAsync();
        await LoadLotsAsync();

        if (ProductDTO.Id > 0)
        {
            selectedLot = lots!.FirstOrDefault(x => x.Id == ProductDTO.LotId)!;
            ProductDTO.Lot = selectedLot;

            selectedSegment = segments!.FirstOrDefault(x => x.Id == ProductDTO!.Classe!.Family!.SegmentId)!;
            await LoadFamilyAsync(selectedSegment.Id);

            selectedFamily = families!.FirstOrDefault(x => x.Id == ProductDTO!.Classe!.FamilyId)!;
            await LoadClasseAsync(selectedFamily.Id);

            selectedClasse = classes!.FirstOrDefault(x => x.Id == ProductDTO!.ClasseId)!;
            ProductDTO.Classe = selectedClasse;

            selectedStatu = status!.FirstOrDefault(x => x.Id == ProductDTO.StatuId)!;
            ProductDTO.Statu = selectedStatu;

            selectedUnitMeasurement = unitMeasurements!.FirstOrDefault(x => x.Id == ProductDTO.UnitMeasurementId)!;
            ProductDTO.UnitMeasurement = selectedUnitMeasurement;

            _disable = false;
        }
        else
        {
            selectedStatu = status!.FirstOrDefault(s => s.Id == 1)!;
            ProductDTO.StatuId = selectedStatu.Id;
            ProductDTO.Statu = selectedStatu;
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

    private async Task LoadUnitMeasurementAsync()
    {
        var responseHttp = await Repository.GetAsync<List<UnitMeasurementDTO>>("/api/unitmeasurements/combo");

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            return;
        }

        unitMeasurements = responseHttp.Response;

    }
    private async Task<IEnumerable<UnitMeasurementDTO>> SearchUnitMeasurement(string searchText, CancellationToken cancellationToken)
    {
        await Task.Delay(5);
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return unitMeasurements!;
        }

        return unitMeasurements!
            .Where(x => x.Name.ToString().Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }
    private void UnitMeasurementChanged(UnitMeasurementDTO entity)
    {
        selectedUnitMeasurement = entity;
        ProductDTO.UnitMeasurementId = entity.Id;
        ProductDTO.UnitMeasurement = entity;
    }

    private async Task LoadSegmentAsync()
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
            .Where(x => x.Name.ToString().Contains(searchText, StringComparison.InvariantCultureIgnoreCase) ||
                        x.Code.ToString().Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }
    private async Task SegmentChanged(SegmentDTO entity)
    {
        selectedSegment = entity;
        selectedFamily = new();
        selectedClasse = new();
        await LoadFamilyAsync(entity.Id);
    }

    private async Task LoadFamilyAsync(int id)
    {
        var responseHttp = await Repository.GetAsync<List<FamilyDTO>>($"/api/families/combo/{id}");

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            return;
        }

        families = responseHttp.Response;
    }
    private async Task<IEnumerable<FamilyDTO>> SearchFamily(string searchText, CancellationToken cancellationToken)
    {
        await Task.Delay(5);
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return families!;
        }

        return families!
            .Where(x => x.Name.ToString().Contains(searchText, StringComparison.InvariantCultureIgnoreCase) ||
                        x.Code.ToString().Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }
    private async Task FamilyChanged(FamilyDTO entity)
    {
        selectedFamily = entity;
        selectedClasse = new();
        await LoadClasseAsync(entity.Id);
    }

    private async Task LoadClasseAsync(int id)
    {
        var responseHttp = await Repository.GetAsync<List<Classe2DTO>>($"/api/classes/combo/{id}");

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            return;
        }

        classes = responseHttp.Response;
    }
    private async Task<IEnumerable<Classe2DTO>> SearchClasse(string searchText, CancellationToken cancellationToken)
    {
        await Task.Delay(5);
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return classes!;
        }

        return classes!
            .Where(x => x.Name.ToString().Contains(searchText, StringComparison.InvariantCultureIgnoreCase) ||
                        x.Code.ToString().Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }
    private void ClasseChanged(Classe2DTO entity)
    {
        selectedClasse = entity;
        ProductDTO.ClasseId = selectedClasse.Id;
        ProductDTO.Classe = selectedClasse;
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
        ProductDTO.StatuId = entity.Id;
        ProductDTO.Statu = entity;
    }

    private async Task LoadLotsAsync()
    {
        var responseHttp = await Repository.GetAsync<List<Lot2DTO>>("/api/lots/combo");

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
        ProductDTO.LotId = entity.Id;
        ProductDTO.Lot = entity;
    }

    private async Task CheckCodeExists(int code)
    {

        selectedSegment = new();
        selectedFamily = new();
        selectedClasse = new();

        if(code== 0) return;

        loading = true;

        string dosPrimeros = string.Concat(code.ToString().Take(2));
        var entity = segments!.Where(x => x.Code== int.Parse(dosPrimeros)).FirstOrDefault();

        if(entity != null)
        {
            selectedSegment = entity!;

            await LoadFamilyAsync(entity.Id);
            string cuartoPrimero = string.Concat(code.ToString().Take(4));
            var familyEntity = families!.Where(x => x.Code == int.Parse(cuartoPrimero)).FirstOrDefault();
            if (familyEntity != null)
            {
                selectedFamily=familyEntity!;
                await LoadClasseAsync(familyEntity!.Id);
                string seisPrimero = string.Concat(code.ToString().Take(6));
                var classeEntity = classes!.Where(x => x.Code == int.Parse(seisPrimero)).FirstOrDefault();
                if (classeEntity != null)
                {
                    selectedClasse = classeEntity!;
                    ProductDTO.ClasseId = selectedClasse.Id;
                    ProductDTO.Classe = selectedClasse;

                    _disable = false;
                }
            }


        }
        loading = false;
    }
}