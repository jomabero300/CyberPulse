using Microsoft.AspNetCore.Identity;

namespace CyberPulse.Backend.Helpers;

public class CustomEmailValidator : IUserValidator<IdentityUser>
{
    private readonly List<string> _disposableEmailDomains = new List<string> { "mailinator.com", "temp-mail.org", "yopmail.com", "tempmail.com", "mailinator.com" }; // Lista de ejemplo
    public async Task<IdentityResult> ValidateAsync(UserManager<IdentityUser> manager, IdentityUser user)
    {
        var email = user.Email;

        if (string.IsNullOrEmpty(email))
        {
            return IdentityResult.Success; // Permitir correos vacíos si es necesario
        }

        var domain = email.Split('@').Last();
        if (_disposableEmailDomains.Contains(domain))
        {
            return IdentityResult.Failed(new IdentityError
            {
                Code = "DisposableEmail",
                Description = "No se permiten correos electrónicos temporales."
            });
        }

        return IdentityResult.Success;
    }
}
