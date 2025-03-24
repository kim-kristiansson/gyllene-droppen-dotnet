using GylleneDroppen.Admin.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

builder.Services.AddControllers();

builder.Services.AddApiServices();

if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers().RequireAuthorization(policy =>
{
    policy.RequireAuthenticatedUser();
    policy.RequireRole("Admin");
});