using CyberPulse.Frontend;
using CyberPulse.Frontend.Respositories;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7233") });
builder.Services.AddScoped<IRepository,Repository>();
builder.Services.AddLocalization();


await builder.Build().RunAsync();
