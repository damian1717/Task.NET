using Microsoft.Extensions.Logging;
using Moq;
using Task.NET.Application.Queries.GetToDoById;
using Task.NET.Domain.Entities;
using Task.NET.Domain.Repositories;
using Task.NET.Domain.ValueObjects.ToDo;
using Task.NET.Shared.Exceptions;

namespace Task.NET.UnitTests.Application.Queries;

public class GetToDoByIdHandlerTests
{
    private readonly Mock<IToDoRepository> _toDoRepositoryMock;
    private readonly Mock<ILogger<GetToDoByIdHandler>> _loggerMock;
    private readonly GetToDoByIdHandler _handler;

    public GetToDoByIdHandlerTests()
    {
        _toDoRepositoryMock = new Mock<IToDoRepository>();
        _loggerMock = new Mock<ILogger<GetToDoByIdHandler>>();
        _handler = new GetToDoByIdHandler(_loggerMock.Object, _toDoRepositoryMock.Object);
    }

    [Fact]
    public async System.Threading.Tasks.Task HandleAsync_ValidId_ShouldReturnToDoDto()
    {
        // Arrange
        var toDoId = Guid.NewGuid();
        var toDo = new ToDo(
            DateTime.Now.AddDays(1),
            new Title("Test Title"),
            new Description("Test Description"),
            50);

        _toDoRepositoryMock
            .Setup(repo => repo.GetByIdAsync(toDoId))
            .ReturnsAsync(toDo);

        var query = new GetToDoByIdQuery(toDoId);

        // Act
        var result = await _handler.HandleAsync(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Test Title", result.Value.Title);
        Assert.Equal("Test Description", result.Value.Description);
        Assert.Equal(50, result.Value.Complete);

        _toDoRepositoryMock.Verify(repo => repo.GetByIdAsync(toDoId), Times.Once);
    }

    [Fact]
    public async System.Threading.Tasks.Task HandleAsync_InvalidId_ShouldThrowNotFoundException()
    {
        // Arrange
        var invalidId = Guid.NewGuid();

        _toDoRepositoryMock
            .Setup(repo => repo.GetByIdAsync(invalidId))
            .ReturnsAsync((ToDo)null);

        var query = new GetToDoByIdQuery(invalidId);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.HandleAsync(query, CancellationToken.None));

        _toDoRepositoryMock.Verify(repo => repo.GetByIdAsync(invalidId), Times.Once);
    }
}
