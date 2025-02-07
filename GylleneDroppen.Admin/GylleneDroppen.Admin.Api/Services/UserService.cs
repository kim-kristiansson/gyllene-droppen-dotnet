using GylleneDroppen.Admin.Api.Dtos;
using GylleneDroppen.Admin.Api.Models;
using GylleneDroppen.Admin.Api.Repositories.Interfaces;
using GylleneDroppen.Admin.Api.Services.Interfaces;
using GylleneDroppen.Admin.Api.Utilities;
using GylleneDroppen.Admin.Api.Utilities.Interfaces;

namespace GylleneDroppen.Admin.Api.Services;

public class UserService(IUserRepository userRepository, IArgon2Hasher argon2Hasher) : IUserService
{
    public async Task<ServiceResponse<MessageResponse>> CreateUserAsync(string email, string password)
    {
        var (hash, salt) = argon2Hasher.HashPassword(password);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = hash,
            PasswordSalt = salt
        };
        
        await userRepository.AddAsync(user);
        await userRepository.SaveChangesAsync();
        
        return ServiceResponse<MessageResponse>.Success(new MessageResponse("User registered successfully. Please check your email for verification."));
    }
}