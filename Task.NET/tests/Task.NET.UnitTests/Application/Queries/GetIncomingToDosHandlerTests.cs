using Microsoft.Extensions.Logging;
using Moq;
using Task.NET.Application.Queries.GetIncomingToDos;
using Task.NET.Domain.Entities;
using Task.NET.Domain.Repositories;
using Task.NET.Domain.ValueObjects.ToDo;

namespace Task.NET.UnitTests.Application.Queries;

public class GetIncomingToDosHandlerTests
{
    private readonly Mock<IToDoRepository> _toDoRepositoryMock;
    private readonly Mock<ILogger<GetIncomingToDosHandler>> _loggerMock;
    private readonly GetIncomingToDosHandler _handler;

    public GetIncomingToDosHandlerTests()
    {
        _toDoRepositoryMock = new Mock<IToDoRepository>();
        _loggerMock = new Mock<ILogger<GetIncomingToDosHandler>>();
        _handler = new GetIncomingToDosHandler(_loggerMock.Object, _toDoRepositoryMock.Object);
    }

    [Fact]
    public async System.Threading.Tasks.Task HandleAsync_ShouldReturnListOfToDoDtos()
    {
        // Arrange
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);
        var endOfWeek = today.AddDays(7 - (int)today.DayOfWeek);

        var toDos = new List<ToDo>
        {
            new ToDo(today.AddHours(10), new Title("Test Title 1"), new Description("Test Description 1"), 50),
            new ToDo(tomorrow.AddHours(12), new Title("Test Title 2"), new Description("Test Description 2"), 75)
        };

        _toDoRepositoryMock
            .Setup(repo => repo.GetIncomingToDosAsync(today, tomorrow, endOfWeek))
            .ReturnsAsync(toDos);

        var query = new GetIncomingToDosQuery();

        // Act
        var result = await _handler.HandleAsync(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Count);
        Assert.Equal("Test Title 1", result.Value[0].Title);
        Assert.Equal("Test Description 1", result.Value[0].Description);
        Assert.Equal(50, result.Value[0].Complete);
        Assert.Equal("Test Title 2", result.Value[1].Title);
        Assert.Equal("Test Description 2", result.Value[1].Description);
        Assert.Equal(75, result.Value[1].Complete);

        _toDoRepositoryMock.Verify(repo => repo.GetIncomingToDosAsync(today, tomorrow, endOfWeek), Times.Once);
    }

    [Fact]
    public async System.Threading.Tasks.Task HandleAsync_WhenNoIncomingToDos_ShouldReturnEmptyList()
    {
        // Arrange
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);
        var endOfWeek = today.AddDays(7 - (int)today.DayOfWeek);

        _toDoRepositoryMock
            .Setup(repo => repo.GetIncomingToDosAsync(today, tomorrow, endOfWeek))
            .ReturnsAsync(new List<ToDo>());

        var query = new GetIncomingToDosQuery();

        // Act
        var result = await _handler.HandleAsync(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value);

        _toDoRepositoryMock.Verify(repo => repo.GetIncomingToDosAsync(today, tomorrow, endOfWeek), Times.Once);
    }
}