using CurrieTechnologies.Razor.SweetAlert2;
using CyberPulse.Frontend;
using CyberPulse.Frontend.AuthenticationProviders;
using CyberPulse.Frontend.Respositories;
using CyberPulse.Frontend.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri("https://api.senagestionformacion.com") });
builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7233") });



//builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri("https://senaraucapi.runasp.net") });

builder.Services.AddScoped<IRepository,Repository>();
builder.Services.AddLocalization();
builder.Services.AddSweetAlert2();
builder.Services.AddMudServices();
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthenticationProviderJWT>();
builder.Services.AddScoped<AuthenticationStateProvider, AuthenticationProviderJWT>(x => x.GetRequiredService<AuthenticationProviderJWT>());
builder.Services.AddScoped<ILoginService, AuthenticationProviderJWT>(x => x.GetRequiredService<AuthenticationProviderJWT>());
builder.Services.AddScoped<ISqlInjValRepository, SqlInjValRepository>();

await builder.Build().RunAsync();
