using Microsoft.Extensions.Logging;
using Moq;
using Task.NET.Application.Commands.UpdateToDo;
using Task.NET.Domain.Entities;
using Task.NET.Domain.Repositories;
using Task.NET.Domain.ValueObjects.ToDo;
using Task.NET.Shared.Exceptions;

namespace Task.NET.UnitTests.Application.Commands;

public class UpdateToDoHandlerTests
{
    private readonly Mock<IToDoRepository> _toDoRepositoryMock;
    private readonly Mock<ILogger<UpdateToDoHandler>> _loggerMock;
    private readonly UpdateToDoHandler _handler;

    public UpdateToDoHandlerTests()
    {
        _toDoRepositoryMock = new Mock<IToDoRepository>();
        _loggerMock = new Mock<ILogger<UpdateToDoHandler>>();
        _handler = new UpdateToDoHandler(_loggerMock.Object, _toDoRepositoryMock.Object);
    }

    [Fact]
    public async System.Threading.Tasks.Task HandleAsync_ValidRequest_ShouldUpdateToDoAndReturnSuccess()
    {
        // Arrange
        var toDoId = Guid.NewGuid();
        var toDo = new ToDo(
            DateTime.Now,
            new Title("Title"),
            new Description("Desc"),
            50);

        _toDoRepositoryMock
            .Setup(repo => repo.GetByIdAsync(toDoId))
            .ReturnsAsync(toDo);

        _toDoRepositoryMock
            .Setup(repo => repo.UpdateAsync(toDo))
            .Returns(System.Threading.Tasks.Task.CompletedTask);

        var command = new UpdateToDoCommand(
            toDoId,
            DateTime.Now.AddDays(1),
            "Updated Title",
            "Updated Description",
            75);

        // Act
        var result = await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Updated Title", toDo.Title.Value);
        Assert.Equal("Updated Description", toDo.Description.Value);
        Assert.Equal(75, toDo.Complete);

        _toDoRepositoryMock.Verify(repo => repo.GetByIdAsync(toDoId), Times.Once);
        _toDoRepositoryMock.Verify(repo => repo.UpdateAsync(toDo), Times.Once);
    }

    [Fact]
    public async System.Threading.Tasks.Task HandleAsync_InvalidId_ShouldThrowNotFoundException()
    {
        // Arrange
        var invalidId = Guid.NewGuid();

        _toDoRepositoryMock
            .Setup(repo => repo.GetByIdAsync(invalidId))
            .ReturnsAsync((ToDo)null);

        var command = new UpdateToDoCommand(
            invalidId,
            DateTime.Now,
            "Updated Title",
            "Desc",
            150);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.HandleAsync(command, CancellationToken.None));

        _toDoRepositoryMock.Verify(repo => repo.GetByIdAsync(invalidId), Times.Once);
        _toDoRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<ToDo>()), Times.Never);
    }

    [Fact]
    public async System.Threading.Tasks.Task HandleAsync_InvalidCompleteValue_ShouldThrowDomainException()
    {
        // Arrange
        var toDoId = Guid.NewGuid();
        var toDo = new ToDo(
            DateTime.Now,
            new Title("Title"),
            new Description("Desc"),
            50);

        _toDoRepositoryMock
            .Setup(repo => repo.GetByIdAsync(toDoId))
            .ReturnsAsync(toDo);

        var command = new UpdateToDoCommand(
            toDoId,
            DateTime.Now,
            "Title",
            "Desc",
            150);

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => _handler.HandleAsync(command, CancellationToken.None));

        _toDoRepositoryMock.Verify(repo => repo.GetByIdAsync(toDoId), Times.Once);
        _toDoRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<ToDo>()), Times.Never);
    }
}
