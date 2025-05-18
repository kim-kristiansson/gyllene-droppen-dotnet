using GylleneDroppen.Blazor.Client.Authentication;
using GylleneDroppen.Blazor.Client.Services;
using GylleneDroppen.Blazor.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using _Imports = GylleneDroppen.Blazor.Client._Imports;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5088/")
});

// Register ApiAuthenticationStateProvider first
builder.Services.AddScoped<ApiAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<ApiAuthenticationStateProvider>());

// Add authentication services
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();
builder.Services.AddAuthorizationCore();

// Then register AuthService
builder.Services.AddScoped<AuthService>();

builder.Services.AddSingleton(services =>
{
    // Create a new configuration dictionary with only the values needed by the client
    var configForWasm = new Dictionary<string, string>
    {
        ["ApiBaseUrl"] = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5088/"
    };

    return configForWasm;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", true);
    app.UseHsts();
}

app.UseHttpsRedirection();

// Add these two lines to enable authentication
app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(_Imports).Assembly);

app.Run();