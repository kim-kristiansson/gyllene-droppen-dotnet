using System.Text;
using GylleneDroppen.Application.Dtos.Auth;
using GylleneDroppen.Application.Interfaces.Services;
using GylleneDroppen.Application.Interfaces.Utilities;
using GylleneDroppen.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace GylleneDroppen.Application.Services;

public class AuthService(
    UserManager<ApplicationUser> userManager,
    IEmailService emailService,
    IUrlGenerator urlGenerator)
    : IAuthService
{
    public async Task RegisterAsync(RegisterRequestDto dto)
    {
        var user = new ApplicationUser
        {
            Email = dto.Email,
            UserName = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName
        };

        var result = await userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));

        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        await emailService.SendEmailConfirmationAsync(user.Email, encodedToken);
    }

    public async Task<bool> ValidateCredentialsAsync(string email, string password)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is not { EmailConfirmed: true })
            return false;

        return await userManager.CheckPasswordAsync(user, password);
    }
}