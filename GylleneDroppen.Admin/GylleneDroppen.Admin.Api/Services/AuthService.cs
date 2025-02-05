using GylleneDroppen.Admin.Api.Repositories.Interfaces;
using GylleneDroppen.Admin.Api.Services.Interfaces;
using GylleneDroppen.Admin.Api.Utilities;
using GylleneDroppen.Admin.Api.Utilities.Interfaces;

namespace GylleneDroppen.Admin.Api.Services;

public class AuthService(IUserService userService, IUserRepository userRepository, IArgon2Hasher argon2Hasher, JsonWebToken jsonWebToken) : IAuthService
{
    public async Task<ServiceResponse<string>> LoginAsync(string email, string password)
    {
        var user = await userRepository.GetByEmailAsync(email);

        if (user is null || argon2Hasher.VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
        {
            return ServiceResponse<string>.Failure("Invalid email or password.", 401);
        }

        var token = jsonWebToken.GenerateToken(user.Id);

        return ServiceResponse<string>.Success(token);
    }

    public async Task<ServiceResponse<bool>> RegisterAsync(string email, string password)
    {
        if(await userRepository.GetByEmailAsync(email) is not null)
            return ServiceResponse<bool>.Failure("Email already exists.", 400);
        
        return await userService.CreateUserAsync(email, password);
    }
}
