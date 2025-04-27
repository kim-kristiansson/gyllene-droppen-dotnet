using GylleneDroppen.Blazor;
using GylleneDroppen.Blazor.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register Services
builder.Services.AddScoped<RegisterService>();
builder.Services.AddScoped<VerificationService>();
builder.Services.AddScoped<AuthService>();

// Register HttpClient
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();