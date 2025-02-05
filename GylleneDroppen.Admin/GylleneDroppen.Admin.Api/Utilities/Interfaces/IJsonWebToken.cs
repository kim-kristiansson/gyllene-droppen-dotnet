namespace GylleneDroppen.Admin.Api.Utilities.Interfaces;

public interface IJsonWebToken
{
    string GenerateToken(Guid userId);
}