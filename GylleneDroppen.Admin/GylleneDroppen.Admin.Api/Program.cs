using DotNetEnv;
using GylleneDroppen.Admin.Api.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

builder.Services.AddOpenApi();

builder.Services.AddConfigureOptions(builder.Configuration);

builder.Services.AddJwtAuthentication();

builder.Services.AddDatabase();

builder.Services.AddRedis(builder.Configuration);

builder.Services.AddDependencyInjections();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseMiddlewares();

app.MapControllers();

app.UseAuthentication();

app.UseHttpsRedirection();

app.Run();