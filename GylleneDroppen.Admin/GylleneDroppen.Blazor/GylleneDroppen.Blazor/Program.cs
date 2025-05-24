using GylleneDroppen.Blazor.Components;
using GylleneDroppen.Blazor.Components.Account;
using GylleneDroppen.Blazor.Email;
using GylleneDroppen.Core.Entities;
using GylleneDroppen.Infrastructure.Authorization;
using GylleneDroppen.Infrastructure.Data;
using GylleneDroppen.Infrastructure.DependencyInjection;
using GylleneDroppen.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using _Imports = GylleneDroppen.Blazor.Client._Imports;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddAuthenticationStateSerialization();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddInfrastructure(builder.Configuration);

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

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
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


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(_Imports).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();