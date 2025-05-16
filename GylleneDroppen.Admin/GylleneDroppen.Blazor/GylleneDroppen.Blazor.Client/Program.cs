using GylleneDroppen.Blazor.Client.Authentication;
using GylleneDroppen.Blazor.Client.Extensions;
using GylleneDroppen.Blazor.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

var apiBaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5088/");
builder.Services.AddScoped(sp => new HttpClient
    {
        BaseAddress = apiBaseAddress
    }
    .EnableCookies());

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();