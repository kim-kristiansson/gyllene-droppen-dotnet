using GylleneDroppen.Blazor.Components;
using GylleneDroppen.Blazor.Client.Services;
using GylleneDroppen.Blazor.Client.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
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

// Register client services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
builder.Services.AddAuthorizationCore();

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

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(_Imports).Assembly);

app.Run();