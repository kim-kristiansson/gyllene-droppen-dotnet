namespace GylleneDroppen.Application.Interfaces.Services;

public interface ICurrentUserService
{
    string? GetUserId();
    string? GetUserEmail();
    bool IsAuthenticated();
}