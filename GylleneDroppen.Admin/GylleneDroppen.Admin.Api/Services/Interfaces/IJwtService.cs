namespace GylleneDroppen.Admin.Api.Services.Interfaces;

public interface IJwtService
{
    string GenerateJwtToken(Guid userId);
}