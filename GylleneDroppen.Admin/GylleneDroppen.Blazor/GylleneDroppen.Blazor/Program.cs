using GylleneDroppen.Application.Interfaces.Services;
using GylleneDroppen.Blazor.Components;
using GylleneDroppen.Blazor.Components.Account;
using GylleneDroppen.Blazor.Email;
using GylleneDroppen.Blazor.Service;
using GylleneDroppen.Core.Entities;
using GylleneDroppen.Infrastructure.Authorization;
using GylleneDroppen.Infrastructure.Data;
using GylleneDroppen.Infrastructure.DependencyInjection;
using GylleneDroppen.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using _Imports = GylleneDroppen.Blazor.Client._Imports;

DotNetEnv.Env.Load(".env");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddAuthenticationStateSerialization();

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<IAppEnvironment, ApplicationEnvironment>();

// Add localization services
builder.Services.AddLocalization();

builder.Logging.AddConsole();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies(options =>
    {
        options.ApplicationCookie?.Configure(cookieOptions =>
        {
            cookieOptions.LoginPath = "/konto/logga-in";
            cookieOptions.AccessDeniedPath = "/konto/nekad-atkomst";
            cookieOptions.ReturnUrlParameter = "returnUrl";
        });
    });

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.Password.RequiredLength = 6;
        options.Password.RequireDigit = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IEmailSender<ApplicationUser>, EmailSender>();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminOnly", policy =>
        policy.Requirements.Add(new AdminRequirement()));

builder.Services.AddScoped<IAuthorizationHandler, AdminAuthorizationHandler>();

var app = builder.Build();

using var scope = app.Services.CreateScope();
try
{
    await DataSeeder.SeedDataAsync(scope.ServiceProvider);
    Console.WriteLine("✅ Data seeding completed successfully");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Error during data seeding: {ex.Message}");
}

// Configure Swedish culture for date/time formatting
var supportedCultures = new[] { new CultureInfo("sv-SE") };
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("sv-SE"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(_Imports).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();