using GylleneDroppen.Application.Interfaces.Public.Mappers;
using GylleneDroppen.Application.Interfaces.Public.Services;
using GylleneDroppen.Application.Interfaces.Repositories;
using GylleneDroppen.Application.Interfaces.Shared.Repositories;
using GylleneDroppen.Application.Interfaces.Shared.Security;
using GylleneDroppen.Application.Interfaces.Shared.Services;
using GylleneDroppen.Application.Interfaces.Shared.Utilities;
using GylleneDroppen.Application.Services.Public;
using GylleneDroppen.Application.Services.Shared;
using GylleneDroppen.Infrastructure.Authentication;
using GylleneDroppen.Infrastructure.Email;
using GylleneDroppen.Infrastructure.Persistence.Repositories.Admin;
using GylleneDroppen.Infrastructure.Persistence.Repositories.Public;
using GylleneDroppen.Infrastructure.Persistence.Repositories.Shared;
using GylleneDroppen.Infrastructure.Redis;
using GylleneDroppen.Infrastructure.Security;
using GylleneDroppen.Presentation.Extensions;
using GylleneDroppen.Presentation.Mappers.Public;
using GylleneDroppen.Presentation.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddOpenApi();

// Add application configuration
builder.Services.AddApplicationConfiguration(builder.Configuration);

// Add Redis
builder.Services.AddRedis(builder.Configuration);

// Add database
builder.Services.AddDatabase();

// Add SmtpClient
builder.Services.AddSmtpClient(builder.Configuration);

// Add repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRedisRepository, RedisRepository>();
builder.Services.AddScoped<ITastingRepository, TastingRepository>();
builder.Services.AddScoped<IAttendeeRepository, AttendeeRepository>();

// Add security services
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtService, JwtService>();

// Add email service
builder.Services.AddScoped<IEmailService, EmailService>();

// Add mapper services
builder.Services.AddScoped<ITastingMapper, TastingMapper>();

// Add utilities
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICookieManager, CookieManager>();

// Add application services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITastingService, TastingService>();

// Add authentication and authorization
builder.Services.AddJwtAuthentication(builder.Configuration);

// Add CORS
builder.Services.AddCustomCors(builder.Configuration);

// Add controllers
builder.Services.AddControllers();

// Build the application
var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();

app.UseCustomCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddlewares();

app.MapControllers();

app.Run();