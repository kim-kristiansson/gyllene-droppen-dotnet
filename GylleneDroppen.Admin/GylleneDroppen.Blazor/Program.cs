using GylleneDroppen.Blazor;
using GylleneDroppen.Blazor.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register the auth handler
builder.Services.AddScoped<AuthenticationHandler>();

// Register Services (ensure AuthService is registered before TokenRefreshService)
builder.Services.AddScoped<RegisterService>();
builder.Services.AddScoped<VerificationService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<TokenRefreshService>();

// Configure HttpClient with the auth handler
builder.Services.AddScoped(sp =>
{
    var handler = sp.GetRequiredService<AuthenticationHandler>();

    // Create HttpClient with auth handler
    var httpClient = new HttpClient(handler)
    {
        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
    };

    return httpClient;
});

var host = builder.Build();

// Initialize token refresh on app start
var tokenService = host.Services.GetRequiredService<TokenRefreshService>();
await tokenService.Initialize();

await host.RunAsync();