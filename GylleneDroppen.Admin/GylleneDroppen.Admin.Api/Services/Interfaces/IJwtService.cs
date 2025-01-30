namespace GylleneDroppen.Admin.Api.Services.Interfaces;

public interface IJwtService
{
    string GenerateToken(Guid userId);
}