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

// Register HttpClient factory
builder.Services.AddHttpClient("AuthAPI");
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient());

// Configure Authentication and Authorization
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

// Register AuthProvider first
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

// Then register services
builder.Services.AddScoped<AppConfigService>();
builder.Services.AddScoped<AuthService>();

// Configure Logging
builder.Logging.SetMinimumLevel(LogLevel.Information);

await builder.Build().RunAsync();