using GylleneDroppen.Application.Dtos.TastingEvent;

namespace GylleneDroppen.Application.Interfaces.Services;

public interface ITastingEventService
{
    Task<TastingEventDto?> GetTastingEventByIdAsync(Guid id);
    Task<List<TastingEventDto>> GetUpcomingTastingEventsAsync(int count = 10);
    Task<List<TastingEventDto>> GetPastTastingEventsAsync(int count = 10);
    Task<List<TastingEventDto>> GetMyTastingEventsAsync(string userId);
    Task<List<TastingEventDto>> GetPublicTastingEventsAsync();
    Task<TastingEventDto> CreateTastingEventAsync(CreateTastingEventRequestDto dto);
    Task<TastingEventDto> UpdateTastingEventAsync(UpdateTastingEventRequestDto dto);
    Task<bool> DeleteTastingEventAsync(Guid id);
    Task<bool> AddWhiskyToEventAsync(AddWhiskyToEventRequestDto dto);
    Task<bool> RemoveWhiskyFromEventAsync(Guid eventId, Guid whiskyId);
    Task<bool> UpdateWhiskyOrderAsync(UpdateWhiskyOrderRequestDto dto);
    Task<bool> RegisterForEventAsync(Guid eventId, string userId);
    Task<bool> UnregisterFromEventAsync(Guid eventId, string userId);
    Task<bool> MarkAttendanceAsync(Guid eventId, string userId, bool attended);
    Task<bool> IsUserRegisteredAsync(Guid eventId, string userId);
}