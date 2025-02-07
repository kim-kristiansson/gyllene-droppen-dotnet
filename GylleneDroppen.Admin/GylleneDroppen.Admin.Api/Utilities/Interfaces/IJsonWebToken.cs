namespace GylleneDroppen.Admin.Api.Utilities.Interfaces;

public interface IJsonWebToken
{
    string GenerateToken(Guid userId);
    Task BlacklistTokenAsync(string token);
    Task<bool> IsTokenBlacklistedAsync(string token);
}