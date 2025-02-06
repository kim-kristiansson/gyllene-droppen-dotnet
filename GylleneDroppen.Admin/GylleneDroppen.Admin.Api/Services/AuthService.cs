using GylleneDroppen.Admin.Api.Dtos;
using GylleneDroppen.Admin.Api.Repositories.Interfaces;
using GylleneDroppen.Admin.Api.Services.Interfaces;
using GylleneDroppen.Admin.Api.Utilities;
using GylleneDroppen.Admin.Api.Utilities.Interfaces;

namespace GylleneDroppen.Admin.Api.Services;

public class AuthService(IUserService userService, IUserRepository userRepository, IArgon2Hasher argon2Hasher, IJsonWebToken jsonWebToken) : IAuthService
{
    public async Task<ServiceResponse<LoginResponse>> LoginAsync(LoginRequest request)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);

        if (user is null || argon2Hasher.VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt))
        {
            return ServiceResponse<LoginResponse>.Failure("Invalid email or password.", 401);
        }

        var token = jsonWebToken.GenerateToken(user.Id);

        return ServiceResponse<LoginResponse>.Success(new LoginResponse
        {
            Id = user.Id,
            Email = user.Email,
            Token = token
        });
    }

    public async Task<ServiceResponse<RegisterResponse>> RegisterAsync(RegisterRequest request)
    {
        if(await userRepository.GetByEmailAsync(request.Email) is not null)
            return ServiceResponse<RegisterResponse>.Failure("Email already exists.", 400);

        return await userService.CreateUserAsync(request.Email, request.Password);
    }
}
