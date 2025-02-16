using GylleneDroppen.Api.Data;
using GylleneDroppen.Api.Models;
using GylleneDroppen.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GylleneDroppen.Api.Repositories;

public class ParticipantRepository(AppDbContext context) : Repository<Participant>(context), IParticipantRepository
{
    public async Task<bool> IsUserRegisteredAsync(Guid userId, Guid eventId)
    {
        return await DbSet
            .AnyAsync(p => p.UserId == userId && p.EventId == eventId);
    }
}