using CyberPulse.Frontend.Respositories;
using CyberPulse.Frontend.Shared;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace CyberPulse.Frontend.Pages.Inve.PurchasingBoardInv;

[Authorize(Roles = "Admi,Purc")]
public partial class PurchasingBoardIndex
{
    private List<ProductQuotationPurcDTO>? ProductQuotations { get; set; }
    private MudTable<ProductQuotationPurcDTO> table = new();
    private readonly int[] pageSizeOptions = { 10, 25, 50, int.MaxValue };
    private int totalRecords = 0;
    private int ValidityId = 0;
    private bool loading;
    private bool lbEsta = false;
    private const string baseUrl = "api/productquotations";
    private string infoFormat = "{first_item}-{last_item} => {all_items}";

    [Inject] private IRepository repository { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Inject] private IDialogService DialogService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    [Parameter, SupplyParameterFromForm] public string Filter { get; set; } = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        await LoadTotalRecordsAsync();
    }
    private async Task LoadTotalRecordsAsync()
    {
        loading = true;

        var url = $"{baseUrl}/TotalRecordsPaginated?Email=Ok";

        if (!string.IsNullOrWhiteSpace(Filter))
        {
            url += $"&filter={Filter}";
        }

        var responseHttp = await repository.GetAsync<int>(url);

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();

            Snackbar.Add(Localizer[message!], Severity.Error);
            return;
        }

        totalRecords = responseHttp.Response;

        loading = false;
    }
    private async Task<TableData<ProductQuotationPurcDTO>> LoadListAsync(TableState state, CancellationToken cancellationToken)
    {
        int page = state.Page + 1;

        int pageSize = state.PageSize;

        var url = $"{baseUrl}/paginated/?page={page}&recordsnumber={pageSize}&Email=Ok";

        if (!string.IsNullOrWhiteSpace(Filter))
        {
            url += $"&filter={Filter}";
        }

        var responseHttp = await repository.GetAsync<List<ProductQuotationPurcDTO>>(url);

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();

            Snackbar.Add(Localizer[message!], Severity.Error);

            return new TableData<ProductQuotationPurcDTO> { Items = [], TotalItems = 0 };
        }

        if (responseHttp.Response == null)
        {
            return new TableData<ProductQuotationPurcDTO> { Items = [], TotalItems = 0 };
        }

        ValidityId = responseHttp.Response.FirstOrDefault()!.ValidityId;

        return new TableData<ProductQuotationPurcDTO>
        {
            Items = responseHttp.Response,

            TotalItems = totalRecords
        };
    }

    private async Task SetFilterValue(string value)
    {
        Filter = value;

        await LoadTotalRecordsAsync();

        await table.ReloadServerData();
    }
    private async Task ShowModalAsync(ProductQuotationPurcDTO entity)
    {
        var options = new DialogOptions()
        {
            CloseOnEscapeKey = true,
            CloseButton = false,
            BackdropClick = false,
            FullWidth = true,
            MaxWidth = MaxWidth.Small
        };

        entity.Estado = true;

        var parameters = new DialogParameters
            {
                { "ProductQuatation", entity }
            };
        IDialogReference dialog = await DialogService.ShowAsync<PurchasingBoardEdit>(
            $"{Localizer["Edit"]} {Localizer["PurchasingBoard"]}", parameters,
            options);

        var result = await dialog.Result;

        if (result!.Canceled)
        {
            await LoadTotalRecordsAsync();

            await table.ReloadServerData();
        }
    }
    private async Task ProcessAllAsync()
    {
        var parameters = new DialogParameters
        {
            { "Message", string.Format(Localizer["ProcessConfirm"],Localizer["Products"],1) }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall, CloseOnEscapeKey = true };

        var dialog = await DialogService.ShowAsync<ConfirmDialog>(Localizer["Confirmation"], parameters, options);

        var result = await dialog.Result;

        if (result!.Canceled )
        {
            return;
        }
        
        var elemento = await repository.GetAsync<bool>($"{baseUrl}/ExistenRows/{ValidityId}/{true}");

        if (elemento.Response)
        {
            var parameters2 = new DialogParameters
            {
                {"Message","Tiene productos sin cotización. Desea pocesarlos? ¡NO podra modificarlos despúes!" }
            };

            dialog = await DialogService.ShowAsync<ConfirmDialog>(Localizer["Confirmation"], parameters2, options);

            result = await dialog.Result;

            if (result!.Canceled)
            {
                return;
            }
        }

        var model = new ProductQuotationPurcDTO();

        model.ValidityId = ValidityId;

        var responseHttp = await repository.PutAsync("/api/productquotations/fulls",model);

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(Localizer[message!], Severity.Error);
            return;
        }
        await LoadTotalRecordsAsync();

        await table.ReloadServerData();

        Snackbar.Add(Localizer["RecordCreateOk"], Severity.Success);
    }
}