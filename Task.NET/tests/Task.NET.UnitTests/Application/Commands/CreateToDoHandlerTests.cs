using Microsoft.Extensions.Logging;
using Moq;
using Task.NET.Application.Commands.CreateToDo;
using Task.NET.Domain.Entities;
using Task.NET.Domain.Repositories;
using Task.NET.Shared.Exceptions;

namespace Task.NET.UnitTests.Application.Commands;

public class CreateToDoHandlerTests
{
    private readonly Mock<IToDoRepository> _toDoRepositoryMock;
    private readonly Mock<ILogger<CreateToDoHandler>> _loggerMock;
    private readonly CreateToDoHandler _handler;

    public CreateToDoHandlerTests()
    {
        _toDoRepositoryMock = new Mock<IToDoRepository>();
        _loggerMock = new Mock<ILogger<CreateToDoHandler>>();
        _handler = new CreateToDoHandler(_loggerMock.Object, _toDoRepositoryMock.Object);
    }

    [Fact]
    public async System.Threading.Tasks.Task HandleAsync_ValidRequest_ShouldReturnSuccessResult()
    {
        // Arrange
        var command = new CreateToDoCommand(
            DateTime.Now.AddDays(1),
            "Test Title",
            "Test Description",
            50);

        var createdToDoId = Guid.NewGuid();
        _toDoRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<ToDo>()))
            .ReturnsAsync(createdToDoId);

        // Act
        var result = await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(createdToDoId, result.Value);
        _toDoRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<ToDo>()), Times.Once);
    }

    [Fact]
    public async System.Threading.Tasks.Task HandleAsync_InvalidCompleteValue_ShouldThrowDomainException()
    {
        // Arrange
        var command = new CreateToDoCommand(
            DateTime.Now.AddDays(1),
            "Test Title",
            "Test Description",
            150);

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => _handler.HandleAsync(command, CancellationToken.None));
        _toDoRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<ToDo>()), Times.Never);
    }
}