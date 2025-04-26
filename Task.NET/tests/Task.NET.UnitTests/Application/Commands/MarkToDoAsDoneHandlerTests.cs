using Microsoft.Extensions.Logging;
using Moq;
using Task.NET.Application.Commands.MarkToDoAsDone;
using Task.NET.Domain.Entities;
using Task.NET.Domain.Repositories;
using Task.NET.Domain.ValueObjects.ToDo;
using Task.NET.Shared.Exceptions;

namespace Task.NET.UnitTests.Application.Commands;

public class MarkToDoAsDoneHandlerTests
{
    private readonly Mock<IToDoRepository> _toDoRepositoryMock;
    private readonly Mock<ILogger<MarkToDoAsDoneHandler>> _loggerMock;
    private readonly MarkToDoAsDoneHandler _handler;

    public MarkToDoAsDoneHandlerTests()
    {
        _toDoRepositoryMock = new Mock<IToDoRepository>();
        _loggerMock = new Mock<ILogger<MarkToDoAsDoneHandler>>();
        _handler = new MarkToDoAsDoneHandler(_loggerMock.Object, _toDoRepositoryMock.Object);
    }

    [Fact]
    public async System.Threading.Tasks.Task HandleAsync_ValidId_ShouldMarkToDoAsDoneAndReturnSuccess()
    {
        // Arrange
        var toDoId = Guid.NewGuid();
        var toDo = new ToDo(
            DateTime.Now,
            new Title("Test Title"),
            new Description("Test Description"),
            0);


        _toDoRepositoryMock
            .Setup(repo => repo.GetByIdAsync(toDoId))
            .ReturnsAsync(toDo);

        _toDoRepositoryMock
            .Setup(repo => repo.UpdateAsync(toDo))
            .Returns(System.Threading.Tasks.Task.CompletedTask);

        var command = new MarkToDoAsDoneCommand (toDoId);

        // Act
        var result = await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(100, toDo.Complete);

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

        var command = new MarkToDoAsDoneCommand(invalidId);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.HandleAsync(command, CancellationToken.None));

        _toDoRepositoryMock.Verify(repo => repo.GetByIdAsync(invalidId), Times.Once);
        _toDoRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<ToDo>()), Times.Never);
    }
}
