using CyberPulse.Frontend.Respositories;
using CyberPulse.Frontend.Services;
using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.EntitiesDTO.Gene;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Net;

namespace CyberPulse.Frontend.Pages.Auth;

[Authorize]
public partial class EditUser
{
    private User? user;
    private List<Country>? countries;
    private bool loading = true;
    private string? imageUrl;
    private Country selectedCountry = new();

    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IDialogService DialogService { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IRepository repository { get; set; } = null!;
    [Inject] private ISqlInjValRepository _sqlValidator { get; set; } = null!;
    [Inject] private ILoginService LoginService { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    protected override async Task OnInitializedAsync()
    {
        await LoadUserAsync();
        await LoadCountiesAsync();

        selectedCountry = user!.Country!;

        if (!string.IsNullOrWhiteSpace(user!.Photo))
        {
            imageUrl = user.Photo;
            user.Photo = null;
        }
    }
    private void ShowModal()
    {
        var closeOnEscapeKey = new DialogOptions() { CloseOnEscapeKey = true };

        DialogService.ShowAsync<ChangePassword>(Localizer["ChangePassword"], closeOnEscapeKey);
    }
    private async Task LoadUserAsync()
    {
        var responseHttp = await repository.GetAsync<User>($"/api/accounts");

        if (responseHttp.Error)
        {
            if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
            {
                NavigationManager.NavigateTo("/");
                return;
            }

            var messageError = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(messageError!, Severity.Error);
            return;
        }

        user = responseHttp.Response;

        loading = false;
    }

    // EditUser.razor.cs
    private void ImageSelected(string imageDataUrl)
    {
        Console.WriteLine($"Recibido: {imageDataUrl?.Substring(0, Math.Min(50, imageDataUrl?.Length ?? 0))}");

        if (string.IsNullOrWhiteSpace(imageDataUrl))
            return;

        // Verificar que sea una Data URL de imagen
        if (!imageDataUrl.StartsWith("data:image/"))
        {
            //Console.WriteLine("No es una imagen válida");
            return;
        }

        // Extraer Base64
        var parts = imageDataUrl.Split(',');
        if (parts.Length != 2)
        {
            //Console.WriteLine("Formato incorrecto");
            return;
        }

        var base64 = parts[1];

        // Guardar Base64
        user!.Photo = base64;

        // Guardar Data URL para mostrar
        imageUrl = imageDataUrl;

        StateHasChanged();

        //Console.WriteLine("Imagen procesada correctamente");
    }

    private async Task LoadCountiesAsync()
    {
        var responseHttp = await repository.GetAsync<List<Country>>("/api/countries/combo");

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(Localizer[message!], Severity.Error);
            return;
        }

        countries = responseHttp.Response;
    }
    private void CountryChanged(Country country)
    {
        selectedCountry = country;
    }
    private async Task<IEnumerable<Country>> SearchCountries(string searchText, CancellationToken cancellationToken)
    {
        await Task.Delay(5);

        if (string.IsNullOrWhiteSpace(searchText))
        {
            return countries!;
        }

        return countries!
            .Where(c => c.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }
    private async Task SaveUserAsync()
    {
        if (_sqlValidator.HasSqlInjection(user!.FirstName) ||
            _sqlValidator.HasSqlInjection(user.LastName) ||
            _sqlValidator.HasSqlInjection(user.PhoneNumber!))
        {
            //Datos del formulario no válidos
            Snackbar.Add(Localizer["ERR010"], Severity.Error);
            return;
        }

        var responseHttp = await repository.PutAsync<User, TokenDTO>("/api/accounts", user!); 

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(Localizer[message!], Severity.Error);
            return;
        }

        await LoginService.LoginAsync(responseHttp.Response!.Token);

        Snackbar.Add(Localizer["RecordSavedOk"], Severity.Success);

        NavigationManager.NavigateTo("/");
    }

    private void ReturnAction()
    {
        NavigationManager.NavigateTo("/");
    }
}