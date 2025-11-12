using CyberPulse.Frontend.Respositories;
using CyberPulse.Frontend.Shared;
using CyberPulse.Shared.Entities.Chipp;
using CyberPulse.Shared.EntitiesDTO.Chipp;
using CyberPulse.Shared.Enums;
using CyberPulse.Shared.Resources;
using CyberPulse.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using MudBlazor;
using System.Net;
using System.Security.Claims;

namespace CyberPulse.Frontend.Pages.Chipp;

[Authorize(Roles = "Admi,Coor,Inst")]
public partial class ChipsIndex
{
    private List<Chip>? chips { get; set; }
    private MudTable<Chip> table = new();
    private readonly int[] pageSizeOptions = { 10, 25, 50, int.MaxValue };
    private int totalRecords = 0;
    private bool loading;
    private const string baseUrl = "api/chips";
    private string infoFormat = "{first_item}-{last_item} => {all_items}";

    [Inject] private IRepository repository { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Inject] private IDialogService DialogService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] IJSRuntime JS { get; set; } = null!;

    [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
    private ClaimsPrincipal? user;
    private string? userId;
    private string? userRollId;
    private int indEsta = 0;
    private bool lbEsta = false;
    [Parameter, SupplyParameterFromForm] public string Filter { get; set; } = string.Empty;


    protected override async Task OnInitializedAsync()
    {
        // Obtener el estado de autenticación

        await LoadTotalRecordsAsync();
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        user = authState.User;
        userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        userRollId = user.FindFirst(ClaimTypes.Role)?.Value;
        if (userRollId != null)
        {
            if (userRollId == "Admi") indEsta = 0;
            if (userRollId == "Coor") indEsta = 1;
            if (userRollId == "Inst") indEsta = 2;
        }
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
    private async Task<TableData<Chip>> LoadListAsync(TableState state, CancellationToken cancellationToken)
    {
        int page = state.Page + 1;

        int pageSize = state.PageSize;

        var url = $"{baseUrl}/paginated/?page={page}&recordsnumber={pageSize}&Email={user!.Identity!.Name}&otro={userRollId}";

        if (!string.IsNullOrWhiteSpace(Filter))
        {
            url += $"&filter={Filter}";
        }

        var responseHttp = await repository.GetAsync<List<Chip>>(url);

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();

            Snackbar.Add(Localizer[message!], Severity.Error);

            return new TableData<Chip> { Items = [], TotalItems = 0 };
        }

        if (responseHttp.Response == null)
        {
            return new TableData<Chip> { Items = [], TotalItems = 0 };
        }

        return new TableData<Chip>
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
        //var options = new DialogOptions() { CloseOnEscapeKey = true, CloseButton = true,FullWidth=true,MaxWidth=MaxWidth.ExtraLarge };
        var options = new DialogOptions() { CloseOnEscapeKey = true, CloseButton = true, BackdropClick = false, FullWidth = true, MaxWidth = MaxWidth.Medium };

        IDialogReference? dialog;

        if (isEdit)
        {

            var parameters = new DialogParameters
            {
                { "Id", id }
            };

            if (user!.IsInRole(UserType.Coor.ToString()))
            {
                dialog = await DialogService.ShowAsync<ChipCoordinatorEdit>(
                $"{Localizer["Edit"]} {Localizer["Chip"]}",
                parameters,
                options);
            }
            else
            {
                dialog = await DialogService.ShowAsync<ChipEdit>(
                $"{Localizer["Edit"]} {Localizer["Chip"]}",
                parameters,
                options);

            }
        }
        else
        {
            if (user!.IsInRole(UserType.Coor.ToString()))
            {
                dialog = await DialogService.ShowAsync<ChipCoordinatorCreate>($"{Localizer["New"]} {Localizer["Chip"]}", options);
            }
            else if (user!.IsInRole(UserType.Inst.ToString()))
            {
                dialog = await DialogService.ShowAsync<ChipCreate>($"{Localizer["New"]} {Localizer["Chip"]}", options);
            }
            else
            {
                dialog = await DialogService.ShowAsync<ChipCreate>($"{Localizer["New"]} {Localizer["Chip"]}", options);
            }
        }

        var result = await dialog.Result;

        if (result!.Canceled)
        {
            await LoadTotalRecordsAsync();

            await table.ReloadServerData();
        }
    }
    private async Task ShowModalOpAsync(int id = 0)
    {

        lbEsta = true;

        var response = ConverAsync(id);

        if (!response.WasSuccess)
        {
            lbEsta = false; 
            return;
        }

        var chipCoordinator = response.Result;

        var responseHttp = await repository.PutAsync("api/chips/fullc/", chipCoordinator);

        if (responseHttp.Error)
        {
            var messageError = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(Localizer[messageError!], Severity.Error);
            lbEsta = false;
            return;
        }

        //1. coordinaor, 2. Instructor
        string messageSend = indEsta.Equals(1) ? "InstructorEmail" : "CoordinatorInfo";
        Snackbar.Add(Localizer[messageSend], Severity.Success);
        await table.ReloadServerData();
    }

    private ActionResponse<ChipCoordinator> ConverAsync(int id,int StatuId=0)
    {
        var tableRow = table.Context.Rows.FirstOrDefault(x => x.Key.Id == id);

        if (!tableRow.Key.idEsta && StatuId<10)
        {
            var message = Localizer["UpdateRow"];
            Snackbar.Add(Localizer[message!], Severity.Warning);
            return new ActionResponse<ChipCoordinator>
                        {
                            WasSuccess = false,
                        };
        }
        else if(string.IsNullOrWhiteSpace(tableRow.Key.ChipNo))
        {
            Snackbar.Add(Localizer["ChipNoNull"], Severity.Error);

            return new ActionResponse<ChipCoordinator>
            {
                WasSuccess = false,
            };
        }


        int statuId = StatuId != 0 ? StatuId : tableRow.Key.StatuId + 1;
        var language = System.Globalization.CultureInfo.CurrentCulture.Name.Substring(0, 2);
        var chipCoordinator = new ChipCoordinator()
        {
            Id = tableRow.Key.Id,
            ChipNo = tableRow.Key.ChipNo,
            Code = "E",
            Identificacion = tableRow.Key.Instructor.DocumentId,
            StartDate = tableRow.Key.StartDate,
            InstructorName = tableRow.Key.Instructor.FullName,
            InstructorId = tableRow.Key.InstructorId,
            ChipProgramId = tableRow.Key.ChipProgramId,
            ChipProgramName = tableRow.Key.ChipProgram.Designation,
            StatuId = statuId,
            idEsta = false,
            language=language,
        };

        return new ActionResponse<ChipCoordinator>
        {
            WasSuccess = true,
            Result = chipCoordinator
        };

    }

    private async Task ActionAsync(string type, int id = 0)
    {
        if(type=="V")
        {
            var report = new ChipReportDTO
            {
                Id=id,
            };

            var response = await repository.GetBytesAsync($"api/chips/reportFull?id={id}&dto=pero");

            if (response.Error || response.Response == null)
            {
                return;
            }

            //await JS.InvokeVoidAsync("mostrarPdfEnNuevaPestana", response.Response);
            await JS.InvokeVoidAsync("displayPdf", response.Response);
        }
        else
        {
            lbEsta = true;

            int statuId = type switch
                                {
                                    "D" => 10,
                                    "E" => 11,
                                    _ => 0
                                };

            if (statuId > 0)
            {
                var response = ConverAsync(id, statuId);

                if (!response.WasSuccess) return;

                var chipCoordinator = response.Result;

                var responseHttp = await repository.PutAsync("api/chips/fullc/", chipCoordinator);

                if (responseHttp.Error)
                {
                    var messageError = await responseHttp.GetErrorMessageAsync();

                    Snackbar.Add(Localizer[messageError!], Severity.Error);
                    return;
                }

                string messageSend = "InstructorEjecutar";
                Snackbar.Add(Localizer[messageSend], Severity.Success);
                await table.ReloadServerData();
            }
        }
        
    }

    private async Task DeleteAsync(Chip entity)
    {
        var parameters = new DialogParameters
        {
            { "Message", string.Format(Localizer["DeleteConfirm"], Localizer["Chip"], entity.ChipNo) }
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
                NavigationManager.NavigateTo("/chips");
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