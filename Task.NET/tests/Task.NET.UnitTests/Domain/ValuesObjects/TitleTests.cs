using Task.NET.Domain.ValueObjects.ToDo;

namespace Task.NET.UnitTests.Domain.ValuesObjects;

public class TitleTests
{
    [Fact]
    public void Title_ShouldAssignValueCorrectly()
    {
        // Arrange
        var value = "Test Title";

        // Act
        var title = new Title(value);

        // Assert
        Assert.Equal(value, title.Value);
    }

    [Fact]
    public void Title_ShouldAllowEmptyValue()
    {
        // Arrange
        var value = string.Empty;

        // Act
        var title = new Title(value);

        // Assert
        Assert.Equal(value, title.Value);
    }

    [Fact]
    public void Title_ShouldAllowNullValue()
    {
        // Arrange
        string? value = null;

        // Act
        var title = new Title(value);

        // Assert
        Assert.Null(title.Value);
    }
}
