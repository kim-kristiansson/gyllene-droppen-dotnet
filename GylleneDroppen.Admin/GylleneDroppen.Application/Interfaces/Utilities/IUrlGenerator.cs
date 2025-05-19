namespace GylleneDroppen.Application.Interfaces.Utilities;

public interface IUrlGenerator
{
    string GenerateEmailConfirmationLink(string email, string encodedToken);
    string GeneratePasswordResetLink(string email, string encodedToken);
}