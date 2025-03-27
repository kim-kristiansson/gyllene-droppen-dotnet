using GylleneDroppen.Core.Domain.Entities;
using GylleneDroppen.Core.Domain.Enums;
using GylleneDroppen.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace GylleneDroppen.Infrastructure.Tests.Persistence.Configurations;

public class AttendeeConfigurationTests : IAsyncLifetime
{
    private readonly TestDatabaseFixture _fixture;
    private DbContextOptions<AppDbContext> _options;

    public AttendeeConfigurationTests()
    {
        _fixture = new TestDatabaseFixture();
    }

    public async Task InitializeAsync()
    {
        await _fixture.InitializeAsync();
        _options = _fixture.ContextOptions;
    }

    public async Task DisposeAsync()
    {
        await _fixture.DisposeAsync();
    }

    [Fact]
    public async Task AttendeeConfiguration_CompositeKeyIsEnforced()
    {
        // Arrange
        var user = await CreateValidUserAsync();
        var tasting = await CreateValidTastingAsync(user.Id);
        var attendee = new Attendee
        {
            UserId = user.Id,
            EventId = tasting.Id,
            RegisteredAt = DateTime.UtcNow
        };

        await using (var context = new AppDbContext(_options))
        {
            context.Attendees.Add(attendee);
            await context.SaveChangesAsync();
        }

        // Try to add duplicate (same composite key)
        var duplicateAttendee = new Attendee
        {
            UserId = user.Id,
            EventId = tasting.Id,
            RegisteredAt = DateTime.UtcNow.AddHours(1)
        };

        // Act & Assert
        using (var context = new AppDbContext(_options))
        {
            context.Attendees.Add(duplicateAttendee);
            await Assert.ThrowsAsync<DbUpdateException>(() => context.SaveChangesAsync());
        }
    }

    [Fact]
    public async Task TastingWithAttendees_CascadeDeleteWorks()
    {
        // Arrange
        var user = await CreateValidUserAsync();
        var tasting = await CreateValidTastingAsync(user.Id);
        var attendee = new Attendee
        {
            UserId = user.Id,
            EventId = tasting.Id,
            RegisteredAt = DateTime.UtcNow
        };

        using (var context = new AppDbContext(_options))
        {
            context.Attendees.Add(attendee);
            await context.SaveChangesAsync();
        }

        // Act - Delete the tasting
        using (var context = new AppDbContext(_options))
        {
            var tastingToDelete = await context.Tastings.FindAsync(tasting.Id);
            context.Tastings.Remove(tastingToDelete);
            await context.SaveChangesAsync();
        }

        // Assert - Attendee should be deleted too
        using (var context = new AppDbContext(_options))
        {
            var attendeeCount = await context.Attendees.CountAsync();
            Assert.Equal(0, attendeeCount);
        }
    }

    [Fact]
    public async Task UserWithAttendees_CascadeDeleteWorks()
    {
        // Arrange
        var user = await CreateValidUserAsync();
        var tasting = await CreateValidTastingAsync(user.Id);
        var attendee = new Attendee
        {
            UserId = user.Id,
            EventId = tasting.Id,
            RegisteredAt = DateTime.UtcNow
        };

        using (var context = new AppDbContext(_options))
        {
            context.Attendees.Add(attendee);
            await context.SaveChangesAsync();
        }

        // Act - Delete the user
        using (var context = new AppDbContext(_options))
        {
            var userToDelete = await context.Users.FindAsync(user.Id);
            context.Users.Remove(userToDelete);
            await context.SaveChangesAsync();
        }

        // Assert - Attendee should be deleted too
        using (var context = new AppDbContext(_options))
        {
            var attendeeCount = await context.Attendees.CountAsync();
            Assert.Equal(0, attendeeCount);
        }
    }

    [Fact]
    public async Task CanRetrieveAttendeeWithNavigationProperties()
    {
        // Arrange
        var user = await CreateValidUserAsync();
        var tasting = await CreateValidTastingAsync(user.Id);
        var attendee = new Attendee
        {
            UserId = user.Id,
            EventId = tasting.Id,
            RegisteredAt = DateTime.UtcNow
        };

        using (var context = new AppDbContext(_options))
        {
            context.Attendees.Add(attendee);
            await context.SaveChangesAsync();
        }

        // Act
        Attendee retrievedAttendee;
        using (var context = new AppDbContext(_options))
        {
            retrievedAttendee = await context.Attendees
                .Include(a => a.User)
                .Include(a => a.Tasting)
                .FirstOrDefaultAsync(a => a.UserId == user.Id && a.EventId == tasting.Id);
        }

        // Assert
        Assert.NotNull(retrievedAttendee);
        Assert.NotNull(retrievedAttendee.User);
        Assert.NotNull(retrievedAttendee.Tasting);
        Assert.Equal(user.Id, retrievedAttendee.User.Id);
        Assert.Equal(user.Email, retrievedAttendee.User.Email);
        Assert.Equal(tasting.Id, retrievedAttendee.Tasting.Id);
        Assert.Equal(tasting.Title, retrievedAttendee.Tasting.Title);
    }

    [Fact]
    public async Task CanQueryAttendeesByUser()
    {
        // Arrange
        var user = await CreateValidUserAsync();
        var tasting1 = await CreateValidTastingAsync(user.Id, "Vinprovning 1");
        var tasting2 = await CreateValidTastingAsync(user.Id, "Vinprovning 2");

        var attendee1 = new Attendee
        {
            UserId = user.Id,
            EventId = tasting1.Id,
            RegisteredAt = DateTime.UtcNow
        };

        var attendee2 = new Attendee
        {
            UserId = user.Id,
            EventId = tasting2.Id,
            RegisteredAt = DateTime.UtcNow.AddHours(1)
        };

        using (var context = new AppDbContext(_options))
        {
            context.Attendees.AddRange(attendee1, attendee2);
            await context.SaveChangesAsync();
        }

        // Act
        List<Attendee> userAttendees;
        using (var context = new AppDbContext(_options))
        {
            userAttendees = await context.Attendees
                .Include(a => a.Tasting)
                .Where(a => a.UserId == user.Id)
                .ToListAsync();
        }

        // Assert
        Assert.Equal(2, userAttendees.Count);
        Assert.Contains(userAttendees, a => a.Tasting.Title == "Vinprovning 1");
        Assert.Contains(userAttendees, a => a.Tasting.Title == "Vinprovning 2");
    }

    private async Task<User> CreateValidUserAsync(string email = "test@example.com")
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "Testsson",
            LastName = "Testsson",
            Email = email,
            PasswordHash = "hashedpassword",
            PasswordSalt = "salt",
            Role = RoleType.User,
            Attendees = new List<Attendee>()
        };

        using var context = new AppDbContext(_options);
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        return user;
    }

    private async Task<Tasting> CreateValidTastingAsync(Guid userId, string title = "Vinprovning")
    {
        var tasting = new Tasting
        {
            Id = Guid.NewGuid(),
            Title = title,
            Description = "En exklusiv vinprovning",
            StartTime = DateTime.UtcNow.AddDays(10),
            EndTime = DateTime.UtcNow.AddDays(10).AddHours(3),
            Location = "Vingården",
            Capacity = 20,
            Price = 50,
            Deadline = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow,
            CreatedById = userId,
            OrganizerId = userId,
            Attendees = new List<Attendee>()
        };

        using var context = new AppDbContext(_options);
        await context.Tastings.AddAsync(tasting);
        await context.SaveChangesAsync();

        return tasting;
    }
}