using GylleneDroppen.Infrastructure.Security;

namespace GylleneDroppen.Infrastructure.Tests.Security;

public class PasswordHasherTests
{
    private readonly PasswordHasher _hasher = new();

    [Fact]
    public void HashPassword_ShouldCreateUniqueHashesForSamePassword()
    {
        // Arrange
        const string password = "TestPassword123!";

        // Act
        var (hash1, salt1) = _hasher.HashPassword(password);
        var (hash2, salt2) = _hasher.HashPassword(password);

        // Assert
        Assert.NotEqual(hash1, hash2); // Hashes should be different due to different random salts
        Assert.NotEqual(salt1, salt2); // Salts should be different
    }

    [Fact]
    public void VerifyPassword_WithCorrectPassword_ShouldReturnTrue()
    {
        // Arrange
        const string password = "TestPassword123!";
        var (hash, salt) = _hasher.HashPassword(password);

        // Act
        var result = _hasher.VerifyPassword(password, hash, salt);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void VerifyPassword_WithIncorrectPassword_ShouldReturnFalse()
    {
        // Arrange
        const string password = "TestPassword123!";
        const string wrongPassword = "WrongPassword123!";
        var (hash, salt) = _hasher.HashPassword(password);

        // Act
        var result = _hasher.VerifyPassword(wrongPassword, hash, salt);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void VerifyPassword_WithSamePasswordDifferentSalt_ShouldReturnFalse()
    {
        // Arrange
        const string password = "TestPassword123!";
        var (hash1, _) = _hasher.HashPassword(password);
        var (_, salt2) = _hasher.HashPassword(password);

        // Act
        var result = _hasher.VerifyPassword(password, hash1, salt2);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void HashPassword_WithInvalidInput_ShouldThrowArgumentException(string? invalidPassword)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _hasher.HashPassword(invalidPassword));
        Assert.Contains("password", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData(null, "validHash", "validSalt")]
    [InlineData("", "validHash", "validSalt")]
    [InlineData(" ", "validHash", "validSalt")]
    public void VerifyPassword_WithInvalidPassword_ShouldThrowArgumentException(string? invalidPassword, string hash,
        string salt)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _hasher.VerifyPassword(invalidPassword, hash, salt));
        Assert.Contains("password", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData("validPassword", null, "validSalt")]
    [InlineData("validPassword", "", "validSalt")]
    [InlineData("validPassword", " ", "validSalt")]
    public void VerifyPassword_WithInvalidHash_ShouldThrowArgumentException(string password, string? invalidHash,
        string salt)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _hasher.VerifyPassword(password, invalidHash, salt));
        Assert.Contains("hash", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData("validPassword", "validHash", null)]
    [InlineData("validPassword", "validHash", "")]
    [InlineData("validPassword", "validHash", " ")]
    public void VerifyPassword_WithInvalidSalt_ShouldThrowArgumentException(string password, string hash,
        string? invalidSalt)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _hasher.VerifyPassword(password, hash, invalidSalt));
        Assert.Contains("salt", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void HashPassword_WithValidPassword_ShouldReturnNonEmptyHashAndSalt()
    {
        // Arrange
        const string password = "ValidPassword123!";

        // Act
        var (hash, salt) = _hasher.HashPassword(password);

        // Assert
        Assert.NotNull(hash);
        Assert.NotNull(salt);
        Assert.NotEmpty(hash);
        Assert.NotEmpty(salt);
    }

    [Fact]
    public void VerifyPassword_WithInvalidBase64Salt_ShouldThrowFormatException()
    {
        // Arrange
        const string password = "ValidPassword123!";
        const string hash = "validBase64Hash==";
        const string invalidBase64Salt = "not-valid-base64";

        // Act & Assert
        Assert.Throws<FormatException>(() =>
            _hasher.VerifyPassword(password, hash, invalidBase64Salt));
    }

    [Fact]
    public void HashPassword_WithLongPassword_ShouldStillWork()
    {
        // Arrange
        var longPassword = new string('a', 1000);

        // Act
        var (hash, salt) = _hasher.HashPassword(longPassword);
        var result = _hasher.VerifyPassword(longPassword, hash, salt);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void VerifyPassword_ConsistentForSameInput()
    {
        // Arrange
        const string password = "TestPassword123!";
        var (hash, salt) = _hasher.HashPassword(password);

        // Act
        var result1 = _hasher.VerifyPassword(password, hash, salt);
        var result2 = _hasher.VerifyPassword(password, hash, salt);

        // Assert
        Assert.True(result1);
        Assert.True(result2);
    }
}