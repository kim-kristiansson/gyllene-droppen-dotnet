using GylleneDroppen.Core.Domain.Entities;
using GylleneDroppen.Core.Domain.Enums;
using GylleneDroppen.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace GylleneDroppen.Infrastructure.Tests.Persistence.Configurations;

public class TastingConfigurationTests : IAsyncLifetime
{
    private readonly TestDatabaseFixture _fixture;
    private DbContextOptions<AppDbContext> _options;

    public TastingConfigurationTests()
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
    public async Task TastingConfiguration_TitleMaxLengthIsEnforced()
    {
        // Arrange
        var user = await CreateValidUserAsync();
        var tasting = new Tasting
        {
            Id = Guid.NewGuid(),
            Title = new string('a', 256), // Exceeds max length of 255
            Description = "En exklusiv vinprovning",
            StartTime = DateTime.UtcNow.AddDays(10),
            EndTime = DateTime.UtcNow.AddDays(10).AddHours(3),
            Location = "Vingården",
            Capacity = 20,
            Price = 50,
            Deadline = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow,
            CreatedById = user.Id,
            OrganizerId = user.Id,
            Attendees = new List<Attendee>()
        };

        // Act & Assert
        using var context = new AppDbContext(_options);
        await Assert.ThrowsAsync<DbUpdateException>(() =>
        {
            context.Tastings.Add(tasting);
            return context.SaveChangesAsync();
        });
    }

    [Fact]
    public async Task TastingConfiguration_DescriptionMaxLengthIsEnforced()
    {
        // Arrange
        var user = await CreateValidUserAsync();
        var tasting = new Tasting
        {
            Id = Guid.NewGuid(),
            Title = "Vinprovning",
            Description = new string('a', 1001), // Exceeds max length of 1000
            StartTime = DateTime.UtcNow.AddDays(10),
            EndTime = DateTime.UtcNow.AddDays(10).AddHours(3),
            Location = "Vingården",
            Capacity = 20,
            Price = 50,
            Deadline = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow,
            CreatedById = user.Id,
            OrganizerId = user.Id,
            Attendees = new List<Attendee>()
        };

        // Act & Assert
        using var context = new AppDbContext(_options);
        await Assert.ThrowsAsync<DbUpdateException>(() =>
        {
            context.Tastings.Add(tasting);
            return context.SaveChangesAsync();
        });
    }

    [Fact]
    public async Task TastingConfiguration_LocationMaxLengthIsEnforced()
    {
        // Arrange
        var user = await CreateValidUserAsync();
        var tasting = new Tasting
        {
            Id = Guid.NewGuid(),
            Title = "Vinprovning",
            Description = "En exklusiv vinprovning",
            StartTime = DateTime.UtcNow.AddDays(10),
            EndTime = DateTime.UtcNow.AddDays(10).AddHours(3),
            Location = new string('a', 501), // Exceeds max length of 500
            Capacity = 20,
            Price = 50,
            Deadline = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow,
            CreatedById = user.Id,
            OrganizerId = user.Id,
            Attendees = new List<Attendee>()
        };

        // Act & Assert
        using var context = new AppDbContext(_options);
        await Assert.ThrowsAsync<DbUpdateException>(() =>
        {
            context.Tastings.Add(tasting);
            return context.SaveChangesAsync();
        });
    }

    [Fact]
    public async Task Tasting_OrganizerDeleteRestricted()
    {
        // Arrange
        var organizer = await CreateValidUserAsync("organizer@example.com");
        var creator = await CreateValidUserAsync("creator@example.com");

        var tasting = new Tasting
        {
            Id = Guid.NewGuid(),
            Title = "Vinprovning",
            Description = "En exklusiv vinprovning",
            StartTime = DateTime.UtcNow.AddDays(10),
            EndTime = DateTime.UtcNow.AddDays(10).AddHours(3),
            Location = "Vingården",
            Capacity = 20,
            Price = 50,
            Deadline = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow,
            CreatedById = creator.Id,
            OrganizerId = organizer.Id,
            Attendees = new List<Attendee>()
        };

        using (var context = new AppDbContext(_options))
        {
            context.Tastings.Add(tasting);
            await context.SaveChangesAsync();
        }

        // Act & Assert - Try to delete organizer
        using (var context = new AppDbContext(_options))
        {
            var organizerToDelete = await context.Users.FindAsync(organizer.Id);
            context.Users.Remove(organizerToDelete);

            // Should throw because of the restrict delete behavior
            await Assert.ThrowsAsync<DbUpdateException>(() => context.SaveChangesAsync());
        }
    }

    [Fact]
    public async Task CanQueryTastingsByDate()
    {
        // Arrange
        var user = await CreateValidUserAsync();
        var now = DateTime.UtcNow;

        var pastTasting = new Tasting
        {
            Id = Guid.NewGuid(),
            Title = "Gammal vinprovning",
            Description = "En avslutad vinprovning",
            StartTime = now.AddDays(-10),
            EndTime = now.AddDays(-10).AddHours(3),
            Location = "Vingården",
            Capacity = 20,
            Price = 50,
            Deadline = now.AddDays(-15),
            CreatedAt = now.AddDays(-20),
            CreatedById = user.Id,
            OrganizerId = user.Id,
            Attendees = new List<Attendee>()
        };

        var futureTasting = new Tasting
        {
            Id = Guid.NewGuid(),
            Title = "Kommande vinprovning",
            Description = "En framtida vinprovning",
            StartTime = now.AddDays(10),
            EndTime = now.AddDays(10).AddHours(3),
            Location = "Vingården",
            Capacity = 30,
            Price = 60,
            Deadline = now.AddDays(7),
            CreatedAt = now.AddDays(-5),
            CreatedById = user.Id,
            OrganizerId = user.Id,
            Attendees = new List<Attendee>()
        };

        using (var context = new AppDbContext(_options))
        {
            await context.Tastings.AddRangeAsync(pastTasting, futureTasting);
            await context.SaveChangesAsync();
        }

        // Act
        List<Tasting> upcomingTastings;
        using (var context = new AppDbContext(_options))
        {
            upcomingTastings = await context.Tastings
                .Where(t => t.StartTime > now)
                .ToListAsync();
        }

        // Assert
        Assert.Single(upcomingTastings);
        Assert.Equal("Kommande vinprovning", upcomingTastings[0].Title);
    }

    [Fact]
    public async Task CanRetrieveTastingWithRelationships()
    {
        // Arrange
        var organizer = await CreateValidUserAsync("organizer@example.com");
        var creator = await CreateValidUserAsync("creator@example.com");

        var tasting = new Tasting
        {
            Id = Guid.NewGuid(),
            Title = "Vinprovning med relationer",
            Description = "Test av relationer",
            StartTime = DateTime.UtcNow.AddDays(10),
            EndTime = DateTime.UtcNow.AddDays(10).AddHours(3),
            Location = "Vingården",
            Capacity = 20,
            Price = 50,
            Deadline = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow,
            CreatedById = creator.Id,
            OrganizerId = organizer.Id,
            Attendees = new List<Attendee>()
        };

        // Act
        using (var context = new AppDbContext(_options))
        {
            context.Tastings.Add(tasting);
            await context.SaveChangesAsync();
        }

        // Assert
        using (var context = new AppDbContext(_options))
        {
            var retrievedTasting = await context.Tastings
                .Include(t => t.Organizer)
                .Include(t => t.CreatedBy)
                .FirstOrDefaultAsync(t => t.Id == tasting.Id);

            Assert.NotNull(retrievedTasting);
            Assert.NotNull(retrievedTasting.Organizer);
            Assert.NotNull(retrievedTasting.CreatedBy);

            Assert.Equal(organizer.Id, retrievedTasting.Organizer.Id);
            Assert.Equal(organizer.Email, retrievedTasting.Organizer.Email);
            Assert.Equal(creator.Id, retrievedTasting.CreatedBy.Id);
            Assert.Equal(creator.Email, retrievedTasting.CreatedBy.Email);
        }
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
}