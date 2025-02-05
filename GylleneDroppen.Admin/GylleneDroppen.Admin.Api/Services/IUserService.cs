using GylleneDroppen.Admin.Api.Models;
using GylleneDroppen.Admin.Api.Repositories.Interfaces;
using GylleneDroppen.Admin.Api.Services.Interfaces;
using GylleneDroppen.Admin.Api.Utilities;
using GylleneDroppen.Admin.Api.Utilities.Interfaces;

namespace GylleneDroppen.Admin.Api.Services;

public class UserService(IUserRepository userRepository, IArgon2Hasher argon2Hasher) : IUserService
{
    public async Task<ServiceResponse<bool>> CreateUserAsync(string email, string password)
    {
        if(await userRepository.GetByEmailAsync(email) is not null)
            return ServiceResponse<bool>.Failure("Email already exists", 401);
        
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
        
        return ServiceResponse<bool>.Success(true);
    }
}