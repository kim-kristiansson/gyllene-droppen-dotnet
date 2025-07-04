using GylleneDroppen.Application.Dtos.Address;
using GylleneDroppen.Application.Dtos.TastingEvent;
using GylleneDroppen.Application.Interfaces.Repositories;
using GylleneDroppen.Application.Interfaces.Services;
using GylleneDroppen.Core.Entities;
using Microsoft.Extensions.Logging;

namespace GylleneDroppen.Application.Services;

public class TastingEventService(
    ITastingEventRepository tastingEventRepository,
    ITastingEventWhiskyRepository tastingEventWhiskyRepository,
    ITastingEventParticipantRepository tastingEventParticipantRepository,
    IWhiskyRepository whiskyRepository,
    ICurrentUserService currentUserService,
    IMembershipService membershipService,
    ILogger<TastingEventService> logger) : ITastingEventService
{
    public async Task<TastingEventDto?> GetTastingEventByIdAsync(Guid id)
    {
        var tastingEvent = await tastingEventRepository.GetTastingEventWithDetailsAsync(id);
        return tastingEvent == null ? null : MapToDto(tastingEvent);
    }

    public async Task<List<TastingEventDto>> GetUpcomingTastingEventsAsync(int count = 10)
    {
        var events = await tastingEventRepository.GetUpcomingTastingEventsAsync(count);
        return events.Select(MapToDto).ToList();
    }

    public async Task<List<TastingEventDto>> GetPastTastingEventsAsync(int count = 10)
    {
        var events = await tastingEventRepository.GetPastTastingEventsAsync(count);
        return events.Select(MapToDto).ToList();
    }

    public async Task<List<TastingEventDto>> GetMyTastingEventsAsync(string userId)
    {
        var events = await tastingEventRepository.GetTastingEventsByOrganizerAsync(userId);
        return events.Select(MapToDto).ToList();
    }

    public async Task<List<TastingEventDto>> GetPublicTastingEventsAsync()
    {
        var events = await tastingEventRepository.GetPublicTastingEventsAsync();
        return events.Select(MapToDto).ToList();
    }

    public async Task<TastingEventDto> CreateTastingEventAsync(CreateTastingEventRequestDto dto)
    {
        var currentUserId = currentUserService.GetUserId();
        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("Användare måste vara inloggad för att skapa provningsevent.");

        var tastingEvent = new TastingEvent
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description,
            EventDate = dto.EventDate.Kind == DateTimeKind.Utc ? dto.EventDate : dto.EventDate.ToUniversalTime(),
            EndTime = dto.EndTime?.Kind == DateTimeKind.Utc ? dto.EndTime : dto.EndTime?.ToUniversalTime(),
            Location = dto.Location,
            AddressId = dto.AddressId,
            MaxParticipants = dto.MaxParticipants,
            IsPublic = dto.IsPublic,
            OrganizedByUserId = currentUserId,
            CreatedDate = DateTime.UtcNow
        };

        await tastingEventRepository.AddAsync(tastingEvent);
        await tastingEventRepository.SaveChangesAsync();

        // Add whiskies if provided
        if (dto.WhiskyIds.Any())
        {
            var whiskies = await whiskyRepository.GetWhiskiesByIdsAsync(dto.WhiskyIds);
            
            int order = 1;
            foreach (var whiskyId in dto.WhiskyIds)
            {
                if (whiskies.Any(w => w.Id == whiskyId))
                {
                    var eventWhisky = new TastingEventWhisky
                    {
                        Id = Guid.NewGuid(),
                        TastingEventId = tastingEvent.Id,
                        WhiskyId = whiskyId,
                        Order = order++,
                        AddedDate = DateTime.UtcNow,
                        AddedByUserId = currentUserId
                    };

                    await tastingEventWhiskyRepository.AddAsync(eventWhisky);
                }
            }

            await tastingEventWhiskyRepository.SaveChangesAsync();
        }

        logger.LogInformation("Tasting event '{EventTitle}' created by user {UserId}", dto.Title, currentUserId);

        var createdEvent = await tastingEventRepository.GetTastingEventWithDetailsAsync(tastingEvent.Id);
        return MapToDto(createdEvent!);
    }

    public async Task<TastingEventDto> UpdateTastingEventAsync(UpdateTastingEventRequestDto dto)
    {
        var currentUserId = currentUserService.GetUserId();
        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("Användare måste vara inloggad för att uppdatera provningsevent.");

        var tastingEvent = await tastingEventRepository.GetByIdAsync(dto.Id);
        if (tastingEvent == null)
            throw new InvalidOperationException("Provningseventet hittades inte.");

        if (tastingEvent.OrganizedByUserId != currentUserId)
            throw new UnauthorizedAccessException("Du kan bara uppdatera dina egna provningsevent.");

        tastingEvent.Title = dto.Title;
        tastingEvent.Description = dto.Description;
        tastingEvent.EventDate = dto.EventDate.Kind == DateTimeKind.Utc ? dto.EventDate : dto.EventDate.ToUniversalTime();
        tastingEvent.EndTime = dto.EndTime?.Kind == DateTimeKind.Utc ? dto.EndTime : dto.EndTime?.ToUniversalTime();
        tastingEvent.Location = dto.Location;
        tastingEvent.AddressId = dto.AddressId;
        tastingEvent.MaxParticipants = dto.MaxParticipants;
        tastingEvent.IsPublic = dto.IsPublic;
        tastingEvent.UpdatedDate = DateTime.UtcNow;

        tastingEventRepository.Update(tastingEvent);
        await tastingEventRepository.SaveChangesAsync();

        logger.LogInformation("Tasting event '{EventId}' updated by user {UserId}", dto.Id, currentUserId);

        var updatedEvent = await tastingEventRepository.GetTastingEventWithDetailsAsync(tastingEvent.Id);
        return MapToDto(updatedEvent!);
    }

    public async Task<bool> DeleteTastingEventAsync(Guid id)
    {
        var currentUserId = currentUserService.GetUserId();
        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("Användare måste vara inloggad för att ta bort provningsevent.");

        var tastingEvent = await tastingEventRepository.GetByIdAsync(id);
        if (tastingEvent == null)
            return false;

        if (tastingEvent.OrganizedByUserId != currentUserId)
            throw new UnauthorizedAccessException("Du kan bara ta bort dina egna provningsevent.");

        tastingEventRepository.Remove(tastingEvent);
        await tastingEventRepository.SaveChangesAsync();

        logger.LogInformation("Tasting event {EventId} deleted by user {UserId}", id, currentUserId);

        return true;
    }

    public async Task<bool> IsUserRegisteredAsync(Guid eventId, string userId)
    {
        return await tastingEventParticipantRepository.IsUserRegisteredAsync(eventId, userId);
    }

    private static TastingEventDto MapToDto(TastingEvent tastingEvent)
    {
        return new TastingEventDto
        {
            Id = tastingEvent.Id,
            Title = tastingEvent.Title,
            Description = tastingEvent.Description,
            EventDate = tastingEvent.EventDate,
            EndTime = tastingEvent.EndTime,
            Location = tastingEvent.Location,
            AddressId = tastingEvent.AddressId,
            Address = tastingEvent.Address != null ? new AddressResponseDto
            {
                Id = tastingEvent.Address.Id,
                Name = tastingEvent.Address.Name,
                StreetAddress = tastingEvent.Address.StreetAddress,
                City = tastingEvent.Address.City,
                PostalCode = tastingEvent.Address.PostalCode,
                Description = tastingEvent.Address.Description,
                IsActive = tastingEvent.Address.IsActive,
                CreatedDate = tastingEvent.Address.CreatedDate,
                UpdatedDate = tastingEvent.Address.UpdatedDate,
                CreatedByUserName = tastingEvent.Address.CreatedByUser?.Email ?? "Okänd",
                UpdatedByUserName = tastingEvent.Address.UpdatedByUser?.Email
            } : null,
            MaxParticipants = tastingEvent.MaxParticipants,
            IsPublic = tastingEvent.IsPublic,
            OrganizedByUserName = tastingEvent.OrganizedByUser?.Email ?? "Okänd",
            CreatedDate = tastingEvent.CreatedDate,
            WhiskyCount = tastingEvent.TastingEventWhiskies.Count,
            ParticipantCount = tastingEvent.Participants.Count,
            Whiskies = tastingEvent.TastingEventWhiskies
                .OrderBy(w => w.Order)
                .Select(w => new TastingEventWhiskyDto
                {
                    Id = w.Id,
                    WhiskyId = w.WhiskyId,
                    WhiskyName = w.Whisky?.Name ?? "Okänd",
                    Distillery = w.Whisky?.Distillery ?? "Okänd",
                    Order = w.Order,
                    Notes = w.Notes
                }).ToList(),
            Participants = tastingEvent.Participants
                .Select(p => new TastingEventParticipantDto
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    UserName = $"{p.User?.FirstName} {p.User?.LastName}".Trim(),
                    UserEmail = p.User?.Email ?? "Okänd",
                    RegisteredDate = p.RegisteredDate,
                    Attended = p.Attended,
                    Notes = p.Notes
                }).ToList()
        };
    }
    
    public async Task<bool> AddWhiskyToEventAsync(AddWhiskyToEventRequestDto dto)
    {
        var currentUserId = currentUserService.GetUserId();
        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("Användare måste vara inloggad.");

        var tastingEvent = await tastingEventRepository.GetTastingEventWithDetailsAsync(dto.TastingEventId);
        if (tastingEvent == null)
            throw new InvalidOperationException("Provningseventet hittades inte.");

        if (tastingEvent.OrganizedByUserId != currentUserId)
            throw new UnauthorizedAccessException("Du kan bara lägga till whiskies till dina egna event.");

        // Check if whisky already exists in event
        if (await tastingEventWhiskyRepository.ExistsAsync(dto.TastingEventId, dto.WhiskyId))
            throw new InvalidOperationException("Denna whisky finns redan i eventet.");

        var whisky = await whiskyRepository.GetByIdAsync(dto.WhiskyId);
        if (whisky == null)
            throw new InvalidOperationException("Whiskyn hittades inte.");

        var existingWhiskies = await tastingEventWhiskyRepository.GetByEventIdAsync(dto.TastingEventId);
        
        var eventWhisky = new TastingEventWhisky
        {
            Id = Guid.NewGuid(),
            TastingEventId = dto.TastingEventId,
            WhiskyId = dto.WhiskyId,
            Order = dto.Order > 0 ? dto.Order : existingWhiskies.Count + 1,
            Notes = dto.Notes,
            AddedDate = DateTime.UtcNow,
            AddedByUserId = currentUserId
        };

        await tastingEventWhiskyRepository.AddAsync(eventWhisky);
        await tastingEventWhiskyRepository.SaveChangesAsync();

        logger.LogInformation("Whisky {WhiskyId} added to event {EventId}", dto.WhiskyId, dto.TastingEventId);

        return true;
    }

    public async Task<bool> RemoveWhiskyFromEventAsync(Guid eventId, Guid whiskyId)
    {
        var currentUserId = currentUserService.GetUserId();
        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("Användare måste vara inloggad.");

        var tastingEvent = await tastingEventRepository.GetByIdAsync(eventId);
        if (tastingEvent == null)
            throw new InvalidOperationException("Provningseventet hittades inte.");

        if (tastingEvent.OrganizedByUserId != currentUserId)
            throw new UnauthorizedAccessException("Du kan bara ta bort whiskies från dina egna event.");

        var eventWhisky = await tastingEventWhiskyRepository.GetByEventAndWhiskyAsync(eventId, whiskyId);
        if (eventWhisky == null)
            return false;

        tastingEventWhiskyRepository.Remove(eventWhisky);
        await tastingEventWhiskyRepository.SaveChangesAsync();

        logger.LogInformation("Whisky {WhiskyId} removed from event {EventId}", whiskyId, eventId);

        return true;
    }

    public async Task<bool> UpdateWhiskyOrderAsync(UpdateWhiskyOrderRequestDto dto)
    {
        var currentUserId = currentUserService.GetUserId();
        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("Användare måste vara inloggad.");

        var tastingEvent = await tastingEventRepository.GetByIdAsync(dto.TastingEventId);
        if (tastingEvent == null)
            throw new InvalidOperationException("Provningseventet hittades inte.");

        if (tastingEvent.OrganizedByUserId != currentUserId)
            throw new UnauthorizedAccessException("Du kan bara uppdatera ordningen för dina egna event.");

        var orders = dto.WhiskyOrders.Select(o => (o.WhiskyId, o.Order)).ToList();
        await tastingEventWhiskyRepository.UpdateOrdersAsync(dto.TastingEventId, orders);
        await tastingEventWhiskyRepository.SaveChangesAsync();

        logger.LogInformation("Whisky order updated for event {EventId}", dto.TastingEventId);

        return true;
    }

    public async Task<bool> RegisterForEventAsync(Guid eventId, string userId)
    {
        var tastingEvent = await tastingEventRepository.GetTastingEventWithDetailsAsync(eventId);
        if (tastingEvent == null)
            throw new InvalidOperationException("Provningseventet hittades inte.");

        if (!tastingEvent.IsPublic && tastingEvent.OrganizedByUserId != userId)
            throw new UnauthorizedAccessException("Detta är ett privat event.");

        var canRegister = await membershipService.CanUserRegisterForEventsAsync(userId);
        if (!canRegister)
        {
            var membershipStatus = await membershipService.GetUserMembershipStatusAsync(userId);
            if (membershipStatus.HasActiveMembership)
            {
                throw new InvalidOperationException("Ditt medlemskap har gått ut. Vänligen förnya det för att registrera dig för event.");
            }
            else if (membershipStatus.HasUsedTrial)
            {
                throw new InvalidOperationException("Du har redan använt din gratis provperiod. Vänligen köp ett medlemskap för att registrera dig för event.");
            }
            else
            {
                throw new InvalidOperationException("Du behöver ett aktivt medlemskap eller en gratis provperiod för att registrera dig för event.");
            }
        }

        if (tastingEvent.MaxParticipants.HasValue)
        {
            var currentCount = await tastingEventParticipantRepository.GetParticipantCountAsync(eventId);
            if (currentCount >= tastingEvent.MaxParticipants.Value)
                throw new InvalidOperationException("Eventet är fullt.");
        }

        if (await tastingEventParticipantRepository.IsUserRegisteredAsync(eventId, userId))
            throw new InvalidOperationException("Du är redan registrerad för detta event.");

        var participant = new TastingEventParticipant
        {
            Id = Guid.NewGuid(),
            TastingEventId = eventId,
            UserId = userId,
            RegisteredDate = DateTime.UtcNow
        };

        await tastingEventParticipantRepository.AddAsync(participant);
        await tastingEventParticipantRepository.SaveChangesAsync();

        await membershipService.UseTrialForEventAsync(userId, eventId);

        logger.LogInformation("User {UserId} registered for event {EventId}", userId, eventId);

        return true;
    }

    public async Task<bool> UnregisterFromEventAsync(Guid eventId, string userId)
    {
        var participant = await tastingEventParticipantRepository.GetByEventAndUserAsync(eventId, userId);
        if (participant == null)
            return false;

        tastingEventParticipantRepository.Remove(participant);
        await tastingEventParticipantRepository.SaveChangesAsync();

        logger.LogInformation("User {UserId} unregistered from event {EventId}", userId, eventId);

        return true;
    }

    public async Task<bool> MarkAttendanceAsync(Guid eventId, string userId, bool attended)
    {
        var currentUserId = currentUserService.GetUserId();
        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("Användare måste vara inloggad.");

        var tastingEvent = await tastingEventRepository.GetByIdAsync(eventId);
        if (tastingEvent == null)
            throw new InvalidOperationException("Provningseventet hittades inte.");

        if (tastingEvent.OrganizedByUserId != currentUserId)
            throw new UnauthorizedAccessException("Endast eventets organisatör kan markera närvaro.");

        var participant = await tastingEventParticipantRepository.GetByEventAndUserAsync(eventId, userId);
        if (participant == null)
            throw new InvalidOperationException("Deltagaren är inte registrerad för detta event.");

        participant.Attended = attended;
        tastingEventParticipantRepository.Update(participant);
        await tastingEventParticipantRepository.SaveChangesAsync();

        logger.LogInformation("Attendance marked as {Attended} for user {UserId} in event {EventId}", 
            attended, userId, eventId);

        return true;
    }
}

