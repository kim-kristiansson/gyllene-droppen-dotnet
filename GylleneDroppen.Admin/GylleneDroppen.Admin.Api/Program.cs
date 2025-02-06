using DotNetEnv;
using GylleneDroppen.Admin.Api.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

builder.Services.AddOpenApi();

builder.Services.AddConfigureOptions(builder.Configuration);

builder.Services.AddJwtAuthentication();

builder.Services.AddDatabase();

builder.Services.AddDependencyInjections();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapControllers();

app.UseHttpsRedirection();

app.Run();