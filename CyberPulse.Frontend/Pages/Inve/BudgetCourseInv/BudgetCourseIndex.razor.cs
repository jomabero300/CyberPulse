using CyberPulse.Frontend.Pages.Inve.BudgetInv;
using CyberPulse.Frontend.Respositories;
using CyberPulse.Frontend.Shared;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO.Chipp;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using MudBlazor;
using System.Net;

namespace CyberPulse.Frontend.Pages.Inve.BudgetCourseInv;

[Authorize(Roles = "Admi")]
public partial class BudgetCourseIndex
{
    private List<BudgetCourse>? BudgetCourses { get; set; }
    private MudTable<BudgetCourse> table = new();
    private readonly int[] pageSizeOptions = { 10, 25, 50, int.MaxValue };
    private int totalRecords = 0;
    private bool loading;
    private bool lbEsta = false;
    private const string baseUrl = "api/budgetcourses";
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
    private async Task<TableData<BudgetCourse>> LoadListAsync(TableState state, CancellationToken cancellationToken)
    {
        int page = state.Page + 1;

        int pageSize = state.PageSize;

        var url = $"{baseUrl}/paginated/?page={page}&recordsnumber={pageSize}";

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

    private async Task ShowModalAsync(int id = 0, bool isEdit = false)
    {
        var options = new DialogOptions() { CloseOnEscapeKey = true, CloseButton = false, BackdropClick = false };

        //IDialogReference? dialog;

        //if (isEdit)
        //{

        //    var parameters = new DialogParameters
        //    {
        //        { "Id", id }
        //    };
        //    dialog = await DialogService.ShowAsync<BudgetCourseEdit>(
        //        $"{Localizer["Edit"]} {Localizer["BudgetCourse"]}",
        //        parameters,
        //        options);
        //}
        //else
        //{
        //    dialog = await DialogService.ShowAsync<BudgetCourseCreate>($"{Localizer["New"]} {Localizer["BudgetCourse"]}", options);
        //}

        IDialogReference? dialog = await DialogService.ShowAsync<BudgetCourseCreate>($"{Localizer["New"]} {Localizer["BudgetCourse"]}", options);

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
    private async Task SendAsync(BudgetCourse model)
    {
        lbEsta = true;

        var language = System.Globalization.CultureInfo.CurrentCulture.Name.Substring(0, 2);

        var modelSend = new BudgetCourseSendDTO
        {
            Id = model.Id,
            InstructorId = model.InstructorId,
            BudgetLotId = model.BudgetLotId,
            ValidityId = model.ValidityId,
            CourseProgramLotId = model.CourseProgramLotId,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            Worth = model.Worth,
            StatuId = 6,
            language = language
        };

        var responseHttp = await repository.PutAsync($"{baseUrl}/fulls/",modelSend);

        if (responseHttp.Error)
        {
            var messageError = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(Localizer[messageError!], Severity.Error);
            lbEsta = false;
            return;
        }

        //1. coordinaor, 2. Instructor
        //string messageSend = indEsta.Equals(1) ? "InstructorEmail" : "CoordinatorInfo";
        Snackbar.Add(Localizer["InstructorCourseEmail"], Severity.Success);
        await table.ReloadServerData();

        lbEsta = false;
    }
    private async Task ShowPdfAsync()
    {
        loading = true;

        var url = "api/budgetcourses/report/true/";

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