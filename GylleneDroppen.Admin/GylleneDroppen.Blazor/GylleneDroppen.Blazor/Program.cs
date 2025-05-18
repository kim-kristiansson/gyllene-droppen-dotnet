using GylleneDroppen.Application.Interfaces.Public.Mappers;
using GylleneDroppen.Application.Interfaces.Public.Services;
using GylleneDroppen.Application.Interfaces.Repositories;
using GylleneDroppen.Application.Interfaces.Services;
using GylleneDroppen.Application.Interfaces.Shared.Repositories;
using GylleneDroppen.Application.Interfaces.Shared.Security;
using GylleneDroppen.Application.Interfaces.Utilities;
using GylleneDroppen.Application.Services;
using GylleneDroppen.Application.Services.Public;
using GylleneDroppen.Blazor.Components;
using GylleneDroppen.Infrastructure.Email;
using GylleneDroppen.Infrastructure.Persistence.Repositories.Admin;
using GylleneDroppen.Infrastructure.Persistence.Repositories.Public;
using GylleneDroppen.Infrastructure.Persistence.Repositories.Shared;
using GylleneDroppen.Infrastructure.Redis;
using GylleneDroppen.Infrastructure.Security;
using GylleneDroppen.Presentation.Extensions;
using GylleneDroppen.Presentation.Mappers.Public;
using GylleneDroppen.Presentation.Utilities;
using _Imports = GylleneDroppen.Blazor.Client._Imports;

var builder = WebApplication.CreateBuilder(args);

// Razor & Blazor services (interactive hybrid)
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// === Custom App Configuration ===
builder.Services.AddApplicationConfiguration(builder.Configuration);
builder.Services.AddRedis(builder.Configuration);
builder.Services.AddDatabase();
builder.Services.AddSmtpClient(builder.Configuration);

// === Dependency Injection ===
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRedisRepository, RedisRepository>();
builder.Services.AddScoped<ITastingRepository, TastingRepository>();
builder.Services.AddScoped<IAttendeeRepository, AttendeeRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITastingService, TastingService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<ICookieManager, CookieManager>();
builder.Services.AddScoped<ITastingMapper, TastingMapper>();
builder.Services.AddHttpContextAccessor();

// === Authentication & Authorization ===
builder.Services.AddAuthentication("GylleneScheme")
    .AddCookie("GylleneScheme", options =>
    {
        options.LoginPath = "/logga-in";
        options.AccessDeniedPath = "/forbidden";
        options.Cookie.Name = "gyllenedroppen.auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.ExpireTimeSpan = TimeSpan.FromHours(2);
    });

builder.Services.AddAuthorization();

// === App Pipeline ===
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging(); // Only used if WASM hydration fails in dev
}
else
{
    app.UseExceptionHandler("/Error", true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Serve from wwwroot
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(_Imports).Assembly);

app.Run();