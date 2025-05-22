using GylleneDroppen.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace GylleneDroppen.Blazor.Components.Account;

// Remove the "else if (EmailSender is IdentityNoOpEmailSender)" block from RegisterConfirmation.razor after updating with a real implementation.
internal sealed class IdentityNoOpEmailSender : IEmailSender<ApplicationUser>
{
    private readonly IEmailSender emailSender = new NoOpEmailSender();

    public Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink) =>
        emailSender.SendEmailAsync(email, "Bekräfta din e-post",
            $"Vänligen bekräfta ditt konto genom att <a href='{confirmationLink}'>klicka här</a>.");

    public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink) =>
        emailSender.SendEmailAsync(email, "Återställ ditt lösenord",
            $"Du kan återställa ditt lösenord genom att <a href='{resetLink}'>klicka här</a>.");

    public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode) =>
        emailSender.SendEmailAsync(email, "Återställ ditt lösenord",
            $"Använd följande kod för att återställa ditt lösenord: {resetCode}");
}