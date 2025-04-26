using Microsoft.Extensions.Logging;
using Moq;
using Task.NET.Application.Commands.DeleteToDo;
using Task.NET.Domain.Entities;
using Task.NET.Domain.Repositories;
using Task.NET.Shared.Exceptions;

namespace Task.NET.UnitTests.Application.Commands;

public class DeleteToDoHandlerTests
{
    private readonly Mock<IToDoRepository> _toDoRepositoryMock;
    private readonly Mock<ILogger<DeleteToDoHandler>> _loggerMock;
    private readonly DeleteToDoHandler _handler;

    public DeleteToDoHandlerTests()
    {
        _toDoRepositoryMock = new Mock<IToDoRepository>();
        _loggerMock = new Mock<ILogger<DeleteToDoHandler>>();
        _handler = new DeleteToDoHandler(_loggerMock.Object, _toDoRepositoryMock.Object);
    }

    [Fact]
    public async System.Threading.Tasks.Task HandleAsync_ValidId_ShouldDeleteToDoAndReturnSuccess()
    {
        // Arrange
        var toDoId = Guid.NewGuid();
        var toDo = new ToDo();

        _toDoRepositoryMock
            .Setup(repo => repo.GetByIdAsync(toDoId))
            .ReturnsAsync(toDo);

        _toDoRepositoryMock
            .Setup(repo => repo.DeleteAsync(toDoId))
            .Returns(System.Threading.Tasks.Task.CompletedTask);

        var command = new DeleteToDoCommand(toDoId);

        // Act
        var result = await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(toDoId, result.Value);

        _toDoRepositoryMock.Verify(repo => repo.GetByIdAsync(toDoId), Times.Once);
        _toDoRepositoryMock.Verify(repo => repo.DeleteAsync(toDoId), Times.Once);
    }

    [Fact]
    public async System.Threading.Tasks.Task HandleAsync_InvalidId_ShouldThrowNotFoundException()
    {
        // Arrange
        var invalidId = Guid.NewGuid();

        _toDoRepositoryMock
            .Setup(repo => repo.GetByIdAsync(invalidId))
            .ReturnsAsync((ToDo)null);

        var command = new DeleteToDoCommand(invalidId);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.HandleAsync(command, CancellationToken.None));

        _toDoRepositoryMock.Verify(repo => repo.GetByIdAsync(invalidId), Times.Once);
        _toDoRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Guid>()), Times.Never);
    }
}