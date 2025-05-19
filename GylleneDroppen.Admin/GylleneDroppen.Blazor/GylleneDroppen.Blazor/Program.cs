using GylleneDroppen.Blazor.Components;
using GylleneDroppen.Core.Entities;
using GylleneDroppen.Infrastructure.DependencyInjection;
using GylleneDroppen.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using _Imports = GylleneDroppen.Blazor.Client._Imports;

var builder = WebApplication.CreateBuilder(args);

// --------------------
// Identity
// --------------------

builder.Services.AddIdentity<User, IdentityRole>(options => { options.SignIn.RequireConfirmedEmail = true; })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
    opt.TokenLifespan = TimeSpan.FromHours(2));

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/logga-in";
    options.LogoutPath = "/logga-ut";
    options.AccessDeniedPath = "/nekad";
});

// ----------------------
// Blazor Hybrid Services
// ----------------------
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddRazorPages();

builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// ---------------------
// Middleware
// ---------------------
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();
app.MapRazorPages();

// ---------------------
// Blazor Hybrid Mapping
// ---------------------
app.MapStaticAssets(); // Must be here to serve WASM assets

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(_Imports).Assembly); // if using Client project

app.Run();