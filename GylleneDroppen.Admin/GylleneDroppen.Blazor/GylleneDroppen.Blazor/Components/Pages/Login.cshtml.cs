using GylleneDroppen.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GylleneDroppen.Blazor.Components.Pages;

public class LoginModel(SignInManager<User> signInManager, UserManager<User> userManager)
    : PageModel
{
    public async Task<IActionResult> OnGetAsync(string email, string returnUrl = "/")
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user is { EmailConfirmed: true }) await signInManager.SignInAsync(user, false);

        // Force a real HTTP redirect — don't return Page()
        return LocalRedirect(returnUrl);
    }
}