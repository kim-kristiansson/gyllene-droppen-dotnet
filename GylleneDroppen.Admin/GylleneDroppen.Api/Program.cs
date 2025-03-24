using DotNetEnv;
using GylleneDroppen.Api.Extensions;
using GylleneDroppen.Shared.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

builder.Services.AddOpenApi();

builder.Services.AddConfigureOptions(builder.Configuration);

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddDatabase();

builder.Services.AddSmtpClient();

builder.Services.AddCustomCors(builder.Configuration);

builder.Services.AddRedis(builder.Configuration);

builder.Services.AddHttpContextAccessor();

builder.Services.AddApiServices();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseCustomCors();

app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();

app.UseMiddlewares();

app.MapControllers();

app.Run();