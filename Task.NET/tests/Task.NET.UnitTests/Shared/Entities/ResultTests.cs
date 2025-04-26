using Task.NET.Shared.Entities;

namespace Task.NET.UnitTests.Shared.Entities;

public class ResultTests
{
    [Fact]
    public void Success_ShouldCreateSuccessfulResult()
    {
        // Arrange
        var value = "Test Value";

        // Act
        var result = Result<string>.Success(value);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(value, result.Value);
        Assert.Equal(string.Empty, result.Error);
    }

    [Fact]
    public void Failure_ShouldCreateFailureResult()
    {
        // Arrange
        var error = "Test Error";

        // Act
        var result = Result<string>.Failure(error);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(default, result.Value);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void Success_ShouldHandleNullValue()
    {
        // Arrange
        string? value = null;

        // Act
        var result = Result<string?>.Success(value);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Null(result.Value);
        Assert.Equal(string.Empty, result.Error);
    }

    [Fact]
    public void Failure_ShouldHandleEmptyError()
    {
        // Arrange
        var error = string.Empty;

        // Act
        var result = Result<string>.Failure(error);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(default, result.Value);
        Assert.Equal(error, result.Error);
    }
}
