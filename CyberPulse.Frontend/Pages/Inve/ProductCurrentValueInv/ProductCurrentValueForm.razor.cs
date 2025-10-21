using CurrieTechnologies.Razor.SweetAlert2;
using CyberPulse.Frontend.Respositories;
using CyberPulse.Shared.EntitiesDTO.Gene;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Localization;

namespace CyberPulse.Frontend.Pages.Inve.ProductCurrentValueInv;

public partial class ProductCurrentValueForm
{
    private EditContext editContext = null!;

    private ValidityDTO selectedValidity = new();
    private List<ValidityDTO>? validities;

    private ProductDTO selectedProduct = new();
    private List<ProductDTO>? products;

    private IvaDTO selectedIva = new();
    private List<IvaDTO>? ivas;
    
    private bool loading;
    //private bool _disable;
    
    [EditorRequired, Parameter] public ProductCurrentValueFormDTO ProductCurrentValueDTO { get; set; } = null!;
    [EditorRequired, Parameter] public EventCallback OnValidSubmit { get; set; }
    [EditorRequired, Parameter] public EventCallback ReturnAction { get; set; }

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    public bool FormPostedSuccessfully { get; set; } = false;
    protected override void OnInitialized()
    {
        editContext = new(ProductCurrentValueDTO);
    }

    protected override async Task OnInitializedAsync()
    {
        loading = true;

        await LoadValidityAsync();
        await LoadIvaAsync();
        await LoadProductAsync();

        if (ProductCurrentValueDTO.Id > 0)
        {
            selectedValidity = validities!.FirstOrDefault(x => x.Id == ProductCurrentValueDTO.ValidityId)!;
            ProductCurrentValueDTO.Validity=selectedValidity;

            selectedIva = ivas!.FirstOrDefault(x => x.Id == ProductCurrentValueDTO.IvaId)!;
            ProductCurrentValueDTO.Iva = selectedIva;

            selectedProduct=products!.FirstOrDefault(x=>x.Id==ProductCurrentValueDTO.ProductId)!;
            ProductCurrentValueDTO.Product = selectedProduct;

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

    private async Task LoadValidityAsync()
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
        ProductCurrentValueDTO.ValidityId = entity.Id;
        ProductCurrentValueDTO.Validity = entity;
    }


    private async Task LoadIvaAsync()
    {
        var responseHttp = await Repository.GetAsync<List<IvaDTO>>("/api/ivas/combo");

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            return;
        }

        ivas = responseHttp.Response;
    }
    private async Task<IEnumerable<IvaDTO>> SearchIva(string searchText, CancellationToken cancellationToken)
    {
        await Task.Delay(5);
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return ivas!;
        }

        return ivas!
            .Where(x => x.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }
    private void IvaChanged(IvaDTO entity)
    {
        selectedIva = entity;
        ProductCurrentValueDTO.IvaId = entity.Id;
        ProductCurrentValueDTO.Iva = entity;
    }

    
    private async Task LoadProductAsync()
    {
        var responseHttp = await Repository.GetAsync<List<ProductDTO>>("/api/products/combo");

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            return;
        }

        products = responseHttp.Response;
    }
    private async Task<IEnumerable<ProductDTO>> SearchProduct(string searchText, CancellationToken cancellationToken)
    {
        await Task.Delay(5);
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return products!;
        }

        return products!
            .Where(x => x.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }
    private void ProductChanged(ProductDTO entity)
    {
        selectedProduct = entity;
        ProductCurrentValueDTO.ProductId = entity.Id;
        ProductCurrentValueDTO.Product = entity;
    }
}