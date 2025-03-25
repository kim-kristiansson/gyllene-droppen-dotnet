using System.Text.RegularExpressions;
using GylleneDroppen.Infrastructure.Common.Utilities;

namespace GylleneDroppen.Infrastructure.Tests.Common.Utilities;

public partial class CodeGeneratorTests
{
    [Theory]
    [InlineData(6)]
    [InlineData(8)]
    [InlineData(10)]
    public void GenerateConfirmationCode_ReturnsCodeWithCorrectLength(int length)
    {
        // Act
        var code = CodeGenerator.GenerateConfirmationCode(length);

        // Assert
        Assert.Equal(length, code.Length);
    }

    [Fact]
    public void GenerateConfirmationCode_ReturnsAlphanumericCharactersOnly()
    {
        // Arrange
        const int length = 10;
        var alphanumericPattern = MyRegex();

        // Act
        var code = CodeGenerator.GenerateConfirmationCode(length);

        // Assert
        Assert.Matches(alphanumericPattern, code);
    }

    [Fact]
    public void GenerateConfirmationCode_GeneratesDifferentCodesOnSubsequentCalls()
    {
        // Arrange
        const int length = 8;
        var generatedCodes = new HashSet<string>();
        const int numberOfCodesToGenerate = 100;

        // Act
        for (var i = 0; i < numberOfCodesToGenerate; i++)
        {
            var code = CodeGenerator.GenerateConfirmationCode(length);
            generatedCodes.Add(code);
        }

        // Assert
        // If all codes are unique, the set should have the same count as the number of iterations
        Assert.Equal(numberOfCodesToGenerate, generatedCodes.Count);
    }

    [Fact]
    public void GenerateConfirmationCode_HasGoodDistributionOfCharacters()
    {
        // Arrange
        const int length = 1000; // Generate a long code to test distribution
        var code = CodeGenerator.GenerateConfirmationCode(length);
        var charCounts = new Dictionary<char, int>();
        const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        // Act
        foreach (var c in code)
        {
            if (!charCounts.TryGetValue(c, out var value))
            {
                value = 0;
                charCounts[c] = value;
            }

            charCounts[c] = ++value;
        }

        // Assert
        // Check that all possible characters are used
        foreach (var c in validChars) Assert.Contains(c, charCounts.Keys);

        // Check that the distribution is reasonably even (no character appears more than 5% of the time)
        var expectedAverageFrequency = 1.0 / validChars.Length;
        const double acceptableDeviation = 0.05; // 5% deviation allowed

        foreach (var frequency in charCounts.Values.Select(count => (double)count / length))
            Assert.True(Math.Abs(frequency - expectedAverageFrequency) < acceptableDeviation,
                $"Character distribution is too uneven. Expected around {expectedAverageFrequency:P2} but got {frequency:P2}");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void GenerateConfirmationCode_WithZeroOrNegativeLength_ReturnsEmptyString(int length)
    {
        // Act
        var code = CodeGenerator.GenerateConfirmationCode(length);

        // Assert
        Assert.Equal(string.Empty, code);
    }

    [GeneratedRegex("^[A-Z0-9]+$")]
    private static partial Regex MyRegex();
}