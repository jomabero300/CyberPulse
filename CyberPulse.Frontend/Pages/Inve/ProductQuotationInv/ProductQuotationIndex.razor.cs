using CyberPulse.Frontend.Pages.Inve.BudgetCourseInv;
using CyberPulse.Frontend.Respositories;
using CyberPulse.Frontend.Shared;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Net;

namespace CyberPulse.Frontend.Pages.Inve.ProductQuotationInv;

[Authorize(Roles = "Admi,Coor,Inst")]
public partial class ProductQuotationIndex
{
    private List<BudgetCourse>? BudgetCourses { get; set; }
    private MudTable<BudgetCourse> table = new();
    private readonly int[] pageSizeOptions = { 10, 25, 50, int.MaxValue };
    private int totalRecords = 0;
    private bool loading;
    private const string baseUrl = "api/budgetcourses";
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
    private async Task<TableData<BudgetCourse>> LoadListAsync(TableState state, CancellationToken cancellationToken)
    {
        int page = state.Page + 1;

        int pageSize = state.PageSize;

        var url = $"{baseUrl}/paginated/?page={page}&recordsnumber={pageSize}&Email=Ok";

        if (!string.IsNullOrWhiteSpace(Filter))
        {
            url += $"&filter={Filter}";
        }

        var responseHttp = await repository.GetAsync<List<BudgetCourse>>(url);

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();

            Snackbar.Add(Localizer[message!], Severity.Error);

            return new TableData<BudgetCourse> { Items = [], TotalItems = 0 };
        }

        if (responseHttp.Response == null)
        {
            return new TableData<BudgetCourse> { Items = [], TotalItems = 0 };
        }

        return new TableData<BudgetCourse>
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

    private async Task ShowModalAsync(BudgetCourse selectedCourse, bool isEdit = false)
    {
        var options = new DialogOptions() { 
            CloseOnEscapeKey = true, 
            CloseButton = false, 
            BackdropClick = false,
            FullWidth = true,
            MaxWidth = MaxWidth.Medium
        };

        IDialogReference? dialog;

        var parameters = new DialogParameters
        {
            { "Id", selectedCourse.Id },
            { "Code",selectedCourse.CourseProgramLot!.Course!.Code },
            { "CourseName",selectedCourse.CourseProgramLot!.Course!.Name },
            { "StartDate",selectedCourse.StartDate!.Value.ToShortDateString() },
            { "EndDate",selectedCourse.EndDate!.Value.ToShortDateString()},
            { "Worth",selectedCourse.Worth.ToString("N2") },
            { "StatuName",selectedCourse.Statu!.Name },
        };

        if (isEdit)
        {

            dialog = await DialogService.ShowAsync<ProductQuotationEdit>(
                $"{Localizer["Edit"]} {Localizer["ProductQuotation"]}",
                parameters,
                options);
        }
        else
        {
            dialog = await DialogService.ShowAsync<ProductQuotationCreate>($"{Localizer["New"]} {Localizer["ProductQuotation"]}", parameters, options);
        }

        var result = await dialog.Result;

        if (result!.Canceled)
        {
            await LoadTotalRecordsAsync();

            await table.ReloadServerData();
        }
    }

    private async Task DeleteAsync(BudgetCourse entity)
    {
        var parameters = new DialogParameters
        {
            { "Message", string.Format(Localizer["DeleteConfirm"], Localizer["BudgetCourse"], entity.Id) }
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
                NavigationManager.NavigateTo("/budgetcourses");
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
}