using CyberPulse.Frontend.Respositories;
using CyberPulse.Frontend.Services;
using CyberPulse.Shared.EntitiesDTO.GeneDTO;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace CyberPulse.Frontend.Pages.Auth;

public partial class Login
{
    private LoginDTO loginDTO = new();
    private bool wasClore;

    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IDialogService DialogService { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IRepository repository { get; set; } = null!;
    [Inject] private ILoginService LoginService { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;

    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

    private void ShowModalRecoverPassword()
    {
        var closeOnEscapeKey = new DialogOptions() { CloseOnEscapeKey = true, MaxWidth = MaxWidth.ExtraExtraLarge };
        DialogService.ShowAsync<RecoverPassword>(Localizer["PasswordRecovery"], closeOnEscapeKey);
    }

    private void ShowModalResendConfirmationEmail()
    {
        var closeOnEscapeKey = new DialogOptions() { CloseOnEscapeKey = true, CloseButton = true, MaxWidth = MaxWidth.ExtraLarge };
        DialogService.ShowAsync<ResendConfirmationEmailToken>(Localizer["MailForwarding"], closeOnEscapeKey);
    }

    private void CloseModal()
    {
        wasClore = true;
        MudDialog.Cancel();
    }

    private async Task LoginAsync()
    {
        if (wasClore)
        {
            NavigationManager.NavigateTo("/");
            return;
        }

        var responseHttp = await repository.PostAsync<LoginDTO, TokenDTO>("/api/accounts/Login", loginDTO);

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(Localizer[message!], Severity.Error);
        }

        await LoginService.LoginAsync(responseHttp.Response!.Token);

        NavigationManager.NavigateTo("/");
    }

}