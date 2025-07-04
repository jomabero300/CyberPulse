using CyberPulse.Frontend.Respositories;
using CyberPulse.Frontend.Shared;
using CyberPulse.Shared.Entities.Chipp;
using CyberPulse.Shared.EntitiesDTO.Chipp;
using CyberPulse.Shared.Enums;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Net;
using System.Security.Claims;

namespace CyberPulse.Frontend.Pages.Chipp;

//[Authorize(Roles = "Admin,inst")]
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

    [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
    private ClaimsPrincipal? user;
    private string? userId;
    private string? userRollId;
    private int indEsta=0;

    private string usuaRole = "";
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
            if(userRollId=="Coor")indEsta =1;
            if(userRollId=="Inst")indEsta =2;
            if(userRollId=="Admi")indEsta =3;
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
        //buscar 
        bool lb = false;

        var responseHttp2 = await repository.GetAsync<ChipCoordinator>($"api/chips/full?id={id}&indEsta={lb}");

        var chipCoordinator = responseHttp2.Response;

        chipCoordinator.Code = "E";
        chipCoordinator.StatuId = 7;

        var responseHttp = await repository.PutAsync("api/chips/fullc/", chipCoordinator);

        if (responseHttp.Error)
        {
            var messageError = await responseHttp.GetErrorMessageAsync();

            Snackbar.Add(Localizer[messageError!], Severity.Error);
            return;
        }


        Snackbar.Add(Localizer["InstructorEmail"], Severity.Success);
        await table.ReloadServerData();
    }
    

    private async Task DeleteAsync(Chip entity)
    {
        var parameters = new DialogParameters
        {
            { "Message", string.Format(Localizer["DeleteConfirm"], Localizer["Statu"], entity.ChipNo) }
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