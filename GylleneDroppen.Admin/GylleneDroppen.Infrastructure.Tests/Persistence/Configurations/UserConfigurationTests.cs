using GylleneDroppen.Core.Domain.Entities;
using GylleneDroppen.Core.Domain.Enums;
using GylleneDroppen.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace GylleneDroppen.Infrastructure.Tests.Persistence.Configurations;

public class UserConfigurationTests(TestDatabaseFixture fixture) : IClassFixture<TestDatabaseFixture>
{
    [Fact]
    public async Task UserConfiguration_EmailMaxLengthIsEnforced()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "Erik",
            LastName = "Johansson",
            Email = new string('a', 321) + "@example.com", // Exceeds max length of 320
            PasswordHash = "hashedpassword",
            PasswordSalt = "salt",
            Role = RoleType.User,
            Attendees = new List<Attendee>()
        };

        // Act & Assert
        using var context = new AppDbContext(fixture.ContextOptions);
        await Assert.ThrowsAsync<DbUpdateException>(() =>
        {
            context.Users.Add(user);
            return context.SaveChangesAsync();
        });
    }

    [Fact]
    public async Task UserConfiguration_PasswordHashMaxLengthIsEnforced()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "Erik",
            LastName = "Johansson",
            Email = "erik.johansson@example.com",
            PasswordHash = new string('a', 256), // Exceeds max length of 255
            PasswordSalt = "salt",
            Role = RoleType.User,
            Attendees = new List<Attendee>()
        };

        // Act & Assert
        using var context = new AppDbContext(fixture.ContextOptions);
        await Assert.ThrowsAsync<DbUpdateException>(() =>
        {
            context.Users.Add(user);
            return context.SaveChangesAsync();
        });
    }

    [Fact]
    public async Task UserConfiguration_PasswordSaltMaxLengthIsEnforced()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "Erik",
            LastName = "Johansson",
            Email = "erik.johansson@example.com",
            PasswordHash = "hashedpassword",
            PasswordSalt = new string('a', 33), // Exceeds max length of 32
            Role = RoleType.User,
            Attendees = new List<Attendee>()
        };

        // Act & Assert
        using var context = new AppDbContext(fixture.ContextOptions);
        await Assert.ThrowsAsync<DbUpdateException>(() =>
        {
            context.Users.Add(user);
            return context.SaveChangesAsync();
        });
    }

    [Fact]
    public async Task UserConfiguration_RequiredFieldsAreEnforced()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "Erik",
            LastName = "Johansson",
            Email = null!, // Required field
            PasswordHash = "hashedpassword",
            PasswordSalt = "salt",
            Role = RoleType.User,
            Attendees = new List<Attendee>()
        };

        // Act & Assert
        using var context = new AppDbContext(fixture.ContextOptions);
        await Assert.ThrowsAsync<DbUpdateException>(() =>
        {
            context.Users.Add(user);
            return context.SaveChangesAsync();
        });
    }

    [Fact]
    public async Task CanAddAndRetrieveUser()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "Testsson",
            LastName = "Testsson",
            Email = "test@example.com",
            PasswordHash = "hashedpassword",
            PasswordSalt = "salt",
            Role = RoleType.User,
            Attendees = new List<Attendee>()
        };

        // Act
        using (var context = new AppDbContext(fixture.ContextOptions))
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        }

        // Assert
        using (var context = new AppDbContext(fixture.ContextOptions))
        {
            var retrievedUser = await context.Users.FindAsync(user.Id);
            Assert.NotNull(retrievedUser);
            Assert.Equal(user.Email, retrievedUser.Email);
            Assert.Equal(user.FirstName, retrievedUser.FirstName);
            Assert.Equal(user.LastName, retrievedUser.LastName);
            Assert.Equal(user.PasswordHash, retrievedUser.PasswordHash);
            Assert.Equal(user.PasswordSalt, retrievedUser.PasswordSalt);
            Assert.Equal(user.Role, retrievedUser.Role);
        }
    }

    [Fact]
    public async Task CanUpdateUserRole()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "Testsson",
            LastName = "Testsson",
            Email = "test2@example.com",
            PasswordHash = "hashedpassword",
            PasswordSalt = "salt",
            Role = RoleType.User,
            Attendees = new List<Attendee>()
        };

        using (var context = new AppDbContext(fixture.ContextOptions))
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        }

        // Act
        using (var context = new AppDbContext(fixture.ContextOptions))
        {
            var retrievedUser = await context.Users.FindAsync(user.Id);
            retrievedUser.Role = RoleType.Admin;
            await context.SaveChangesAsync();
        }

        // Assert
        using (var context = new AppDbContext(fixture.ContextOptions))
        {
            var updatedUser = await context.Users.FindAsync(user.Id);
            Assert.Equal(RoleType.Admin, updatedUser.Role);
        }
    }
}