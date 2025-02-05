using DotNetEnv;
using GylleneDroppen.Admin.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

builder.Services.AddOpenApi();

builder.Services.AddConfigureOptions(builder.Configuration);

builder.Services.AddJwtAuthentication();

builder.Services.AddDatabase();

builder.Services.AddDependencyInjections();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();