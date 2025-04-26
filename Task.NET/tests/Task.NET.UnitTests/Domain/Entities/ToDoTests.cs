using Task.NET.Domain.Entities;
using Task.NET.Domain.ValueObjects.ToDo;
using Task.NET.Shared.Exceptions;

namespace Task.NET.UnitTests.Domain.Entities;

public class ToDoTests
{
    [Fact]
    public void Create_ValidParameters_ShouldCreateToDo()
    {
        // Arrange
        var dateTimeOfExpiry = DateTime.Now.AddDays(1);
        var title = new Title("Test Title");
        var description = new Description("Test Description");
        var complete = 50;

        // Act
        var toDo = ToDo.Create(dateTimeOfExpiry, title, description, complete);

        // Assert
        Assert.Equal(dateTimeOfExpiry, toDo.DateTimeOfExpiry);
        Assert.Equal(title, toDo.Title);
        Assert.Equal(description, toDo.Description);
        Assert.Equal(complete, toDo.Complete);
    }

    [Fact]
    public void UpdateDateTimeOfExpiry_ShouldUpdateValue()
    {
        // Arrange
        var toDo = new ToDo();
        var newDateTime = DateTime.Now.AddDays(2);

        // Act
        toDo.UpdateDateTimeOfExpiry(newDateTime);

        // Assert
        Assert.Equal(newDateTime, toDo.DateTimeOfExpiry);
    }

    [Fact]
    public void UpdateTitle_ShouldUpdateValue()
    {
        // Arrange
        var toDo = new ToDo();
        var newTitle = new Title("Updated Title");

        // Act
        toDo.UpdateTitle(newTitle);

        // Assert
        Assert.Equal(newTitle, toDo.Title);
    }

    [Fact]
    public void UpdateDescription_ShouldUpdateValue()
    {
        // Arrange
        var toDo = new ToDo();
        var newDescription = new Description("Updated Description");

        // Act
        toDo.UpdateDescription(newDescription);

        // Assert
        Assert.Equal(newDescription, toDo.Description);
    }

    [Fact]
    public void UpdateComplete_ValidValue_ShouldUpdateValue()
    {
        // Arrange
        var toDo = new ToDo();
        var newComplete = 75;

        // Act
        toDo.UpdateComplete(newComplete);

        // Assert
        Assert.Equal(newComplete, toDo.Complete);
    }

    [Fact]
    public void UpdateComplete_InvalidValue_ShouldThrowDomainException()
    {
        // Arrange
        var toDo = new ToDo();

        // Act & Assert
        Assert.Throws<DomainException>(() => toDo.UpdateComplete(-1));
        Assert.Throws<DomainException>(() => toDo.UpdateComplete(101));
    }
}