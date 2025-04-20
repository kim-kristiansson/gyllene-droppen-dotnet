using GylleneDroppen.Admin.Api.Extensions;
using GylleneDroppen.Presentation.Extensions;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddServices();

builder.Services.AddApplicationConfiguration(builder.Configuration);

builder.Services.AddRedis(builder.Configuration);

builder.Services.AddDatabase();

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddCustomCors(builder.Configuration);

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("RequireAdminRole", policy =>
        policy.RequireRole("Admin"))
    .SetFallbackPolicy(new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .RequireRole("Admin")
        .Build());

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();

app.UseCustomCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddlewares();

app.MapControllers();

app.Run();