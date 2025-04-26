using Task.NET.Domain.ValueObjects.ToDo;

namespace Task.NET.UnitTests.Domain.ValuesObjects;

public class DescriptionTests
{
    [Fact]
    public void Description_ShouldAssignValueCorrectly()
    {
        // Arrange
        var value = "Test Description";

        // Act
        var description = new Description(value);

        // Assert
        Assert.Equal(value, description.Value);
    }

    [Fact]
    public void Description_ShouldAllowEmptyValue()
    {
        // Arrange
        var value = string.Empty;

        // Act
        var description = new Description(value);

        // Assert
        Assert.Equal(value, description.Value);
    }

    [Fact]
    public void Description_ShouldAllowNullValue()
    {
        // Arrange
        string? value = null;

        // Act
        var description = new Description(value);

        // Assert
        Assert.Null(description.Value);
    }
}
