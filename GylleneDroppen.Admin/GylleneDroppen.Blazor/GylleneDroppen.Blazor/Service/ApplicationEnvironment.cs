using GylleneDroppen.Application.Interfaces.Services;

namespace GylleneDroppen.Blazor.Service;

public class ApplicationEnvironment(IWebHostEnvironment env) : IAppEnvironment
{
    public string WebRootPath => env.WebRootPath;
}