using GylleneDroppen.Core.Domain.Entities;
using GylleneDroppen.Core.Domain.Enums;
using GylleneDroppen.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace GylleneDroppen.Infrastructure.Tests.Persistence.Data;

public class AppDbContextTests
{
    private readonly DbContextOptions<AppDbContext> _options = new DbContextOptionsBuilder<AppDbContext>()
        .UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}")
        .Options;

    [Fact]
    public async Task CanAddAndRetrieveUser()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "Erik",
            LastName = "Johansson",
            Email = "erik.johansson@example.com",
            PasswordHash = "hashedpassword",
            PasswordSalt = "salt",
            Role = RoleType.User
        };

        // Act
        await using (var context = new AppDbContext(_options))
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        }

        // Assert
        await using (var context = new AppDbContext(_options))
        {
            var retrievedUser = await context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            Assert.NotNull(retrievedUser);
            Assert.Equal(user.Email, retrievedUser.Email);
            Assert.Equal(user.FirstName, retrievedUser.FirstName);
            Assert.Equal(user.LastName, retrievedUser.LastName);
        }
    }

    [Fact]
    public async Task CanAddAndRetrieveTastingWithRelationships()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PasswordHash = "hashedpassword",
            PasswordSalt = "salt",
            Role = RoleType.User
        };

        var organizer = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "Anna",
            LastName = "Lindberg",
            Email = "anna.lindberg@example.com",
            PasswordHash = "hashedpassword",
            PasswordSalt = "salt",
            Role = RoleType.Admin
        };

        var tasting = new Tasting
        {
            Id = Guid.NewGuid(),
            Title = "Wine Tasting Event",
            Description = "A fancy wine tasting event",
            StartTime = DateTime.UtcNow.AddDays(10),
            EndTime = DateTime.UtcNow.AddDays(10).AddHours(3),
            Location = "Winery",
            Capacity = 20,
            Price = 50,
            Deadline = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow,
            CreatedById = user.Id,
            OrganizerId = organizer.Id,
            Attendees = []
        };

        // Act
        await using (var context = new AppDbContext(_options))
        {
            await context.Users.AddRangeAsync(user, organizer);
            await context.Tastings.AddAsync(tasting);
            await context.SaveChangesAsync();
        }

        // Assert
        await using (var context = new AppDbContext(_options))
        {
            var retrievedTasting = await context.Tastings
                .Include(t => t.CreatedBy)
                .Include(t => t.Organizer)
                .FirstOrDefaultAsync(t => t.Id == tasting.Id);

            Assert.NotNull(retrievedTasting);
            Assert.Equal(tasting.Title, retrievedTasting.Title);
            Assert.Equal(tasting.Description, retrievedTasting.Description);

            // Verify relationships
            Assert.NotNull(retrievedTasting.CreatedBy);
            Assert.Equal(user.Id, retrievedTasting.CreatedBy.Id);
            Assert.Equal(user.Email, retrievedTasting.CreatedBy.Email);

            Assert.NotNull(retrievedTasting.Organizer);
            Assert.Equal(organizer.Id, retrievedTasting.Organizer.Id);
            Assert.Equal(organizer.Email, retrievedTasting.Organizer.Email);
        }
    }

    [Fact]
    public async Task CanRegisterAttendeeForTasting()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PasswordHash = "hashedpassword",
            PasswordSalt = "salt",
            Role = RoleType.User
        };

        var organizer = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@example.com",
            PasswordHash = "hashedpassword",
            PasswordSalt = "salt",
            Role = RoleType.Admin
        };

        var attendee = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "Maja",
            LastName = "Andersson",
            Email = "maja.andersson@example.com",
            PasswordHash = "hashedpassword",
            PasswordSalt = "salt",
            Role = RoleType.User
        };

        var tasting = new Tasting
        {
            Id = Guid.NewGuid(),
            Title = "Wine Tasting Event",
            Description = "A fancy wine tasting event",
            StartTime = DateTime.UtcNow.AddDays(10),
            EndTime = DateTime.UtcNow.AddDays(10).AddHours(3),
            Location = "Winery",
            Capacity = 20,
            Price = 50,
            Deadline = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow,
            CreatedById = user.Id,
            OrganizerId = organizer.Id,
            Attendees = []
        };

        var attendance = new Attendee
        {
            UserId = attendee.Id,
            EventId = tasting.Id,
            RegisteredAt = DateTime.UtcNow
        };

        // Act
        await using (var context = new AppDbContext(_options))
        {
            await context.Users.AddRangeAsync(user, organizer, attendee);
            await context.Tastings.AddAsync(tasting);
            await context.SaveChangesAsync();

            await context.Attendees.AddAsync(attendance);
            await context.SaveChangesAsync();
        }

        // Assert
        await using (var context = new AppDbContext(_options))
        {
            var retrievedTasting = await context.Tastings
                .Include(t => t.Attendees)
                .ThenInclude(a => a.User)
                .FirstOrDefaultAsync(t => t.Id == tasting.Id);

            Assert.NotNull(retrievedTasting);
            Assert.Single(retrievedTasting.Attendees);
            Assert.Equal(attendee.Id, retrievedTasting.Attendees.First().UserId);
            Assert.Equal(attendee.Email, retrievedTasting.Attendees.First().User?.Email);

            // Check from the user side as well
            var retrievedAttendee = await context.Users
                .Include(u => u.Attendees)
                .ThenInclude(a => a.Tasting)
                .FirstOrDefaultAsync(u => u.Id == attendee.Id);

            Assert.NotNull(retrievedAttendee);
            Assert.Single(retrievedAttendee.Attendees);
            Assert.Equal(tasting.Id, retrievedAttendee.Attendees.First().EventId);
            Assert.Equal(tasting.Title, retrievedAttendee.Attendees.First().Tasting?.Title);
        }
    }
}