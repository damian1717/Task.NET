using Task.NET.Shared.Provider;

namespace Task.NET.UnitTests.Shared.Provider;

public class AssembliesProviderTests
{
    [Fact]
    public void Get_ShouldReturnAssemblies_WhenValidPrefixIsProvided()
    {
        // Arrange
        var allowedPrefix = new List<string> { "Task.NET" };

        // Act
        var assemblies = AssembliesProvider.Get(allowedPrefix);

        // Assert
        Assert.NotNull(assemblies);
        Assert.All(assemblies, assembly =>
        {
            Assert.Contains("Task.NET", assembly.FullName, StringComparison.InvariantCultureIgnoreCase);
        });
    }

    [Fact]
    public void Get_ShouldThrowArgumentException_WhenAllowedPrefixIsNull()
    {
        // Arrange
        List<string>? allowedPrefix = null;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => AssembliesProvider.Get(allowedPrefix!));
        Assert.Equal("AllowedPrefix is null.", exception.Message);
    }

    [Fact]
    public void Get_ShouldReturnEmpty_WhenNoMatchingAssembliesFound()
    {
        // Arrange
        var allowedPrefix = new List<string> { "NonExistentPrefix" };

        // Act
        var assemblies = AssembliesProvider.Get(allowedPrefix);

        // Assert
        Assert.NotNull(assemblies);
        Assert.Empty(assemblies);
    }

    [Fact]
    public void IsAllowed_ShouldReturnTrue_WhenNameMatchesPrefix()
    {
        // Arrange
        var fileName = "Task.NET.Shared.dll";
        var allowedPrefix = new List<string> { "Task.NET" };

        // Act
        var result = fileName.IsAllowed(allowedPrefix);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsAllowed_ShouldReturnFalse_WhenNameDoesNotMatchPrefix()
    {
        // Arrange
        var fileName = "OtherNamespace.Shared.dll";
        var allowedPrefix = new List<string> { "Task.NET" };

        // Act
        var result = fileName.IsAllowed(allowedPrefix);

        // Assert
        Assert.False(result);
    }
}