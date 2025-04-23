using GylleneDroppen.Admin.Blazor;
using GylleneDroppen.Admin.Blazor.Auth;
using GylleneDroppen.Admin.Blazor.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register HttpClient
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Register Services
builder.Services.AddScoped<AppConfigService>();
builder.Services.AddScoped<AuthService>();

// Configure Logging
builder.Logging.SetMinimumLevel(LogLevel.Information);

// Register Authentication and Authorization
builder.Services.AddAuthorizationCore(options =>
{
    // Add a specific policy for admin users
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));

    // Make all routes require authentication by default
    // The Login page will be explicitly excluded in App.razor
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

await builder.Build().RunAsync();