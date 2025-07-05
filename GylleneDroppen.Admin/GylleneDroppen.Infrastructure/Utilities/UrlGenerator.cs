using GylleneDroppen.Application.Interfaces.Utilities;
using GylleneDroppen.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace GylleneDroppen.Infrastructure.Utilities;

public class UrlGenerator(IOptions<GlobalSettings> options) : IUrlGenerator
{
    private readonly GlobalSettings _settings = options.Value;

    public string GenerateEmailConfirmationLink(string email, string encodedToken)
    {
        return $"{_settings.BaseUrl}/verifiera-epost?epost={email}&kod={encodedToken}";
    }

    public string GeneratePasswordResetLink(string email, string encodedToken)
    {
        return $"{_settings.BaseUrl}/aterstall-losenord?epost={email}&kod={encodedToken}";
    }
}