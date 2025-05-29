namespace GylleneDroppen.Application.Services;

public interface ICurrentUserService
{
    string? GetUserId();
    string? GetUserEmail();
    bool IsAuthenticated();
}