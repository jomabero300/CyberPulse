using CurrieTechnologies.Razor.SweetAlert2;
using CyberPulse.Frontend.Pages.Chipp;
using CyberPulse.Frontend.Respositories;
using CyberPulse.Shared.Entities.Chipp;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace CyberPulse.Frontend.Layout;

public partial class MainLayout
{
    [Inject] private IRepository repository { get; set; } = null!;
    [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
    [Inject] private IDialogService DialogService { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;


    private bool _drawerOpen = true;
    private string _icon = Icons.Material.Filled.DarkMode;
    private bool _darkMode { get; set; } = true;

    private bool HayAlertas = false;
    private List<Chip>? chips;

    protected override async Task OnInitializedAsync()
    {
        await LoadAlertAsync();
    }

    private async Task LoadAlertAsync()
    {
        string language = System.Globalization.CultureInfo.CurrentCulture.Name.Substring(0, 2);
        var responseHttp = await repository.GetAsync<List<Chip>>($"/api/chips/verificar/{language}");

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            //await SweetAlertService.FireAsync("Error", Localizer[message!], SweetAlertIcon.Error);
            return;
        }

        HayAlertas = responseHttp.Response!.Any();

        chips = responseHttp.Response;
    }



    private void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }

    private void DarkModeToggle()
    {
        _darkMode = !_darkMode;
        _icon = _darkMode ? Icons.Material.Filled.LightMode : Icons.Material.Filled.DarkMode;
    }
    private async Task MostrarModalAlertas()
    {
        var parameters = new DialogParameters { ["chips"] = chips };

        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.Small,
            CloseOnEscapeKey = true,
            FullWidth = true
        };

        await DialogService.ShowAsync<ChipFormAlert>(Localizer["SentAlert"], parameters, options);
    }
}