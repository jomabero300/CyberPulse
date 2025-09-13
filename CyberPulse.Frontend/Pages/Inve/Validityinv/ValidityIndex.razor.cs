using CyberPulse.Frontend.Respositories;
using CyberPulse.Frontend.Shared;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Net;

namespace CyberPulse.Frontend.Pages.Inve.Validityinv;

[Authorize(Roles = "Admi")]
public partial class ValidityIndex
{
    private List<Validity>? validities { get; set; }
    private MudTable<Validity> table = new();
    private readonly int[] pageSizeOptions = { 10, 25, 50, int.MaxValue };
    private int totalRecords = 0;
    private bool loading;
    private const string baseUrl = "api/validities";
    private string infoFormat = "{first_item}-{last_item} => {all_items}";
    private bool lbNew;

    [Inject] private IRepository repository { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Inject] private IDialogService DialogService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    [Parameter, SupplyParameterFromForm] public string Filter { get; set; } = string.Empty;
    protected override async Task OnInitializedAsync()
    {
        var responseHttp = await repository.GetAsync("/api/validities/Validez");

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            if (message == "ERRO12") 
            {
                lbNew=true;
            }
        }
        await LoadTotalRecordsAsync();
    }
    private async Task LoadTotalRecordsAsync()
    {
        loading = true;

        var url = $"{baseUrl}/TotalRecordsPaginated";

        if (!string.IsNullOrWhiteSpace(Filter))
        {
            url += $"?filter={Filter}";
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
    private async Task<TableData<Validity>> LoadListAsync(TableState state, CancellationToken cancellationToken)
    {
        int page = state.Page + 1;

        int pageSize = state.PageSize;

        var url = $"{baseUrl}/paginated/?page={page}&recordsnumber={pageSize}";

        if (!string.IsNullOrWhiteSpace(Filter))
        {
            url += $"&filter={Filter}";
        }

        var responseHttp = await repository.GetAsync<List<Validity>>(url);

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();

            Snackbar.Add(Localizer[message!], Severity.Error);

            return new TableData<Validity> { Items = [], TotalItems = 0 };
        }

        if (responseHttp.Response == null)
        {
            return new TableData<Validity> { Items = [], TotalItems = 0 };
        }

        return new TableData<Validity>
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
    private async Task ShowModalAsync(int id = 0, bool isEdit = false)
    {
        var options = new DialogOptions() { CloseOnEscapeKey = true, CloseButton = false, BackdropClick = false };

        IDialogReference? dialog;

        if (isEdit)
        {

            var parameters = new DialogParameters
            {
                { "Id", id }
            };
            dialog = await DialogService.ShowAsync<ValidityEdit>(
                $"{Localizer["Edit"]} {Localizer["Validity"]}",
                parameters,
                options);
        }
        else
        {
            dialog = await DialogService.ShowAsync<ValidityCreate>($"{Localizer["New"]} {Localizer["Validity"]}", options);
        }

        var result = await dialog.Result;

        if (result!.Canceled)
        {
            await LoadTotalRecordsAsync();

            await table.ReloadServerData();
        }
    }
    private async Task DeleteAsync(Validity entity)
    {
        var parameters = new DialogParameters
        {
            { "Message", string.Format(Localizer["DeleteConfirm"], Localizer["Validity"], entity.Value) }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall, CloseOnEscapeKey = true };

        var dialog = await DialogService.ShowAsync<ConfirmDialog>(Localizer["Confirmation"], parameters, options);

        var result = await dialog.Result;

        if (result!.Canceled)
        {
            return;
        }


        var responseHttp = await repository.DeleteAsync($"{baseUrl}/full/{entity.Id}");

        if (responseHttp.Error)
        {
            if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
            {
                NavigationManager.NavigateTo("/validities");
            }
            else
            {
                var message = await responseHttp.GetErrorMessageAsync();
                Snackbar.Add(Localizer[message!], Severity.Error);
            }
            return;
        }

        await LoadTotalRecordsAsync();

        await table.ReloadServerData();

        Snackbar.Add(Localizer["RecordDeletedOk"], Severity.Success);
    }

    private async Task CloseValidity(Validity validity)
    {
        var parameters = new DialogParameters
        {
            { "Message", string.Format(Localizer["CloseValidityConfirm"], Localizer["CloseValidity"], validity.Value) }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall, CloseOnEscapeKey = true };

        var dialog = await DialogService.ShowAsync<ConfirmDialog>(Localizer["Confirmation"], parameters, options);

        var result = await dialog.Result;

        if (result!.Canceled)
        {
            return;
        }


        var validityDTO = new ValidityDTO
        {
            Value = validity.Value,
            Id = validity.Id,
            IsInvalid = false,
            StatuId = 6
        };

        var responseHttp = await repository.PutAsync($"{baseUrl}/full/", validityDTO);

        if (responseHttp.Error)
        {
            if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
            {
                NavigationManager.NavigateTo("/validities");
            }
            else
            {
                var message = await responseHttp.GetErrorMessageAsync();
                Snackbar.Add(Localizer[message!], Severity.Error);
            }
            return;
        }

        await LoadTotalRecordsAsync();

        await table.ReloadServerData();

        Snackbar.Add(Localizer["RecordCloseValidityConfirmdOk"], Severity.Success);

    }
}