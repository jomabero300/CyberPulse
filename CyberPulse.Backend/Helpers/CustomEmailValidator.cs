using CyberPulse.Shared.Entities.Gene;
using Microsoft.AspNetCore.Identity;

namespace CyberPulse.Backend.Helpers;

//public class CustomEmailValidator : IUserValidator<IdentityUser>
public class CustomEmailValidator : IUserValidator<User>
{
    //private readonly List<string> _disposableEmailDomains = new List<string> { "mailinator.com", "temp-mail.org", "yopmail.com", "tempmail.com", "mailinator.com" }; // Lista de ejemplo

    //public async Task<IdentityResult> ValidateAsync(UserManager<IdentityUser> manager, IdentityUser user)
    //{
    //    var email = user.Email;

    //    if (string.IsNullOrEmpty(email))
    //    {
    //        //return IdentityResult.Success; // Permitir correos vacíos si es necesario

    //        return IdentityResult.Failed(new IdentityError
    //        {
    //            Code = "InvalidEmailDomain",
    //            Description = "El correo electrónico es obligatorio y debe ser de un dominio válido."
    //        });
    //    }

    //    var domain = email.Split('@').Last();

    //    if (_disposableEmailDomains.Contains(domain))
    //    {
    //        return IdentityResult.Failed(new IdentityError
    //        {
    //            Code = "DisposableEmail",
    //            Description = "No se permiten correos electrónicos temporales."
    //            //Code = "InvalidEmailDomain",
    //            //Description = "El correo electrónico debe ser de un dominio permitido (sena.edu.co)."
    //        });
    //    }

    //    return IdentityResult.Success;
    //}

    //private readonly List<string> _allowedEmailDomains = new List<string> { "sena.edu.co", "sedarauca.edu.co" };
    //public Task<IdentityResult> ValidateAsync(UserManager<IdentityUser> manager, IdentityUser user)
    //{
    //    var email = user.Email;

    //    if (string.IsNullOrEmpty(email))
    //    {
    //        return Task.FromResult(IdentityResult.Failed(new IdentityError
    //        {
    //            Code = "InvalidEmailDomain",
    //            Description = "El correo electrónico es obligatorio y debe ser de un dominio válido."
    //        }));
    //    }

    //    var domain = email.Split('@').LastOrDefault();

    //    if (string.IsNullOrEmpty(domain) || !_allowedEmailDomains.Contains(domain.ToLowerInvariant()))
    //    {
    //        return Task.FromResult(IdentityResult.Failed(new IdentityError
    //        {
    //            Code = "InvalidEmailDomain",
    //            Description = "El correo electrónico debe ser de un dominio permitido (sena.edu.co)."
    //        }));
    //    }

    //    return Task.FromResult(IdentityResult.Success);
    //}

    private readonly List<string> _allowedEmailDomains = new List<string> { "sena.edu.co", "sedarauca.edu.co"};

    public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user)
    {
        var email = user.Email;

        if (string.IsNullOrEmpty(email))
        {
            return Task.FromResult(IdentityResult.Failed(new IdentityError
            {
                Code = "EmailDomainInvalid",
                Description = "El correo electrónico es obligatorio y debe ser de un dominio válido."
            }));
        }

        var domain = email.Split('@').LastOrDefault();

        if (string.IsNullOrEmpty(domain) || !_allowedEmailDomains.Contains(domain.ToLowerInvariant()))
        {
            return Task.FromResult(IdentityResult.Failed(new IdentityError
            {
                Code = "EmailDomainInvalid",
                Description = "El correo electrónico debe ser de un dominio permitido (sena.edu.co o sedarauca.edu.co)."
            }));
        }

        return Task.FromResult(IdentityResult.Success);
    }
}
