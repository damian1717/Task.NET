using Microsoft.Extensions.Logging;
using Moq;
using Task.NET.Application.Queries.GetAllToDos;
using Task.NET.Domain.Entities;
using Task.NET.Domain.Repositories;
using Task.NET.Domain.ValueObjects.ToDo;

namespace Task.NET.UnitTests.Application.Queries;

public class GetAllToDosHandlerTests
{
    private readonly Mock<IToDoRepository> _toDoRepositoryMock;
    private readonly Mock<ILogger<GetAllToDosHandler>> _loggerMock;
    private readonly GetAllToDosHandler _handler;

    public GetAllToDosHandlerTests()
    {
        _toDoRepositoryMock = new Mock<IToDoRepository>();
        _loggerMock = new Mock<ILogger<GetAllToDosHandler>>();
        _handler = new GetAllToDosHandler(_loggerMock.Object, _toDoRepositoryMock.Object);
    }

    [Fact]
    public async System.Threading.Tasks.Task HandleAsync_ShouldReturnListOfToDoDtos()
    {
        // Arrange
        var toDos = new List<ToDo>
        {
            new ToDo(DateTime.Now.AddDays(1), new Title("Test Title 1"), new Description("Test Description 1"), 50),
            new ToDo(DateTime.Now.AddDays(2), new Title("Test Title 2"), new Description("Test Description 2"), 75)
        };

        _toDoRepositoryMock
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(toDos);

        var query = new GetAllToDosQuery();

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

        _toDoRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async System.Threading.Tasks.Task HandleAsync_WhenNoToDosExist_ShouldReturnEmptyList()
    {
        // Arrange
        _toDoRepositoryMock
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(new List<ToDo>());

        var query = new GetAllToDosQuery();

        // Act
        var result = await _handler.HandleAsync(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value);

        _toDoRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }
}