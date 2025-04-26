using Task.NET.Application.Dtos;

namespace Task.NET.UnitTests.Application.Dtos;

public class ToDoDtoTests
{
    [Fact]
    public void ToDoDto_ShouldAssignPropertiesCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dateTimeOfExpiry = DateTime.Now.AddDays(1);
        var title = "Test Title";
        var description = "Test Description";
        var complete = 50;

        // Act
        var toDoDto = new ToDoDto(id, dateTimeOfExpiry, title, description, complete);

        // Assert
        Assert.Equal(id, toDoDto.Id);
        Assert.Equal(dateTimeOfExpiry, toDoDto.DateTimeOfExpiry);
        Assert.Equal(title, toDoDto.Title);
        Assert.Equal(description, toDoDto.Description);
        Assert.Equal(complete, toDoDto.Complete);
    }
}