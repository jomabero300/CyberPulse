using CyberPulse.Frontend.Respositories;
using CyberPulse.Frontend.Shared;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using MudBlazor;
using System.Net;

namespace CyberPulse.Frontend.Pages.Inve.BudgetProgramInv;

[Authorize(Roles = "Admi")]
public partial class BudgetProgramIndex
{
    private List<BudgetProgramIndexDTO>? budgetPrograms { get; set; }
    private MudTable<BudgetProgramIndexDTO> table = new();
    private readonly int[] pageSizeOptions = { 10, 25, 50, int.MaxValue };
    private int totalRecords = 0;
    private bool loading;
    private const string baseUrl = "api/budgetprograms";
    private string infoFormat = "{first_item}-{last_item} => {all_items}";

    [Inject] private IRepository repository { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Inject] private IDialogService DialogService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] IJSRuntime JS { get; set; } = null!;
    [Parameter, SupplyParameterFromForm] public string Filter { get; set; } = string.Empty;
    protected override async Task OnInitializedAsync()
    {
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
    private async Task<TableData<BudgetProgramIndexDTO>> LoadListAsync(TableState state, CancellationToken cancellationToken)
    {
        int page = state.Page + 1;

        int pageSize = state.PageSize;

        var url = $"{baseUrl}/paginated/?page={page}&recordsnumber={pageSize}";

        if (!string.IsNullOrWhiteSpace(Filter))
        {
            url += $"&filter={Filter}";
        }

        var responseHttp = await repository.GetAsync<List<BudgetProgramIndexDTO>>(url);

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();

            Snackbar.Add(Localizer[message!], Severity.Error);

            return new TableData<BudgetProgramIndexDTO> { Items = [], TotalItems = 0 };
        }

        if (responseHttp.Response == null)
        {
            return new TableData<BudgetProgramIndexDTO> { Items = [], TotalItems = 0 };
        }

        var groupedByProgram = responseHttp.Response.ToList().GroupBy(bp => bp.ProgramId);

        foreach (var group in groupedByProgram)
        {
            if (group.Count() > 1)
            {
                var orderedPrograms = group.OrderBy(bp => bp.Id).ToList();

                var lastProgram = orderedPrograms.Last();

                foreach (var programToUpdate in orderedPrograms.Where(p => p.Id != lastProgram.Id))
                {
                    programToUpdate.StatuId = 0;
                }
                lastProgram.StatuId = 1;
            }
        }

        return new TableData<BudgetProgramIndexDTO>
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
        //TODO: Validar si hay vigencia activa y Tipos de presupuesto

        var options = new DialogOptions() { CloseOnEscapeKey = true, CloseButton = false, BackdropClick = false, FullWidth = true, MaxWidth = MaxWidth.Small };

        IDialogReference? dialog;

        if (isEdit)
        {

            var parameters = new DialogParameters
            {
                { "Id", id }
            };
            dialog = await DialogService.ShowAsync<BudgetProgramEdit>(
                $"{Localizer["Edit"]} {Localizer["BudgetProgram"]}",
                parameters,
                options);
        }
        else
        {
            dialog = await DialogService.ShowAsync<BudgetProgramCreate>($"{Localizer["New"]} {Localizer["BudgetProgram"]}", options);
        }

        var result = await dialog.Result;

        if (result!.Canceled)
        {
            await LoadTotalRecordsAsync();

            await table.ReloadServerData();
        }
    }
    private async Task DeleteAsync(BudgetProgramIndexDTO entity)
    {
        var parameters = new DialogParameters
        {
            { "Message", string.Format(Localizer["DeleteConfirm"], Localizer["BudgetType"],$"{entity.Program!.Name} de la vigencia {entity.Validity!.Value}") }
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
                NavigationManager.NavigateTo("/budgetprograms");
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
    private async Task ShowPdfAsync()
    {
        loading = true;

        var url = "api/budgetprograms/report/";

        if (!string.IsNullOrEmpty(Filter))
        {
            url += $"{Filter}";
        }
        else
        {
            url += "''";
        }

        var response = await repository.GetBytesAsync(url);

        if (response.Error || response.Response == null)
        {
            // Handle error
            loading = false;
            return;
        }

        await JS.InvokeVoidAsync("displayPdf", response.Response);

        loading = false;
    }
}