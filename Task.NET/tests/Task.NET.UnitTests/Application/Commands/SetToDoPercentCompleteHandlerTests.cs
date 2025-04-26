using Microsoft.Extensions.Logging;
using Moq;
using Task.NET.Application.Commands.SetToDoPercentComplate;
using Task.NET.Domain.Entities;
using Task.NET.Domain.Repositories;
using Task.NET.Domain.ValueObjects.ToDo;
using Task.NET.Shared.Exceptions;

namespace Task.NET.UnitTests.Application.Commands;

public class SetToDoPercentCompleteHandlerTests
{
    private readonly Mock<IToDoRepository> _toDoRepositoryMock;
    private readonly Mock<ILogger<SetToDoPercentCompleteHandler>> _loggerMock;
    private readonly SetToDoPercentCompleteHandler _handler;

    public SetToDoPercentCompleteHandlerTests()
    {
        _toDoRepositoryMock = new Mock<IToDoRepository>();
        _loggerMock = new Mock<ILogger<SetToDoPercentCompleteHandler>>();
        _handler = new SetToDoPercentCompleteHandler(_loggerMock.Object, _toDoRepositoryMock.Object);
    }

    [Fact]
    public async System.Threading.Tasks.Task HandleAsync_ValidIdAndPercentComplete_ShouldUpdateToDoAndReturnSuccess()
    {
        // Arrange
        var toDoId = Guid.NewGuid();
        var toDo = new ToDo(
            DateTime.Now,
            new Title("Test"),
            new Description("Test"),
            50);

        _toDoRepositoryMock
            .Setup(repo => repo.GetByIdAsync(toDoId))
            .ReturnsAsync(toDo);

        _toDoRepositoryMock
            .Setup(repo => repo.UpdateAsync(toDo))
            .Returns(System.Threading.Tasks.Task.CompletedTask);

        var command = new SetToDoPercentCompleteCommand(
            toDoId,
            75);

        // Act
        var result = await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
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

        var command = new SetToDoPercentCompleteCommand(
            invalidId,
            75);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.HandleAsync(command, CancellationToken.None));

        _toDoRepositoryMock.Verify(repo => repo.GetByIdAsync(invalidId), Times.Once);
        _toDoRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<ToDo>()), Times.Never);
    }

    [Fact]
    public async System.Threading.Tasks.Task HandleAsync_InvalidPercentComplete_ShouldThrowDomainException()
    {
        // Arrange
        var toDoId = Guid.NewGuid();
        var toDo = new ToDo(
            DateTime.Now, 
            new Title("Test"), 
            new Description("Test"),
            0);

        _toDoRepositoryMock
            .Setup(repo => repo.GetByIdAsync(toDoId))
            .ReturnsAsync(toDo);

        var command = new SetToDoPercentCompleteCommand(
            toDoId,
            150);

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => _handler.HandleAsync(command, CancellationToken.None));

        _toDoRepositoryMock.Verify(repo => repo.GetByIdAsync(toDoId), Times.Once);
        _toDoRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<ToDo>()), Times.Never);
    }
}
