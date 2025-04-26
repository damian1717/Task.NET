using Microsoft.EntityFrameworkCore;
using Task.NET.Domain.Entities;
using Task.NET.Domain.ValueObjects.ToDo;
using Task.NET.Infrastructure.Repositories;
using Task.NET.Infrastructure;
using Task.NET.Shared.Exceptions;
using Task.NET.Shared.Interceptors;

namespace Task.NET.UnitTests.Infrastructure.Repositories;

public class ToDoRepositoryTests
{
    private readonly DbContextOptions<AppDbContext> _dbContextOptions;

    public ToDoRepositoryTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async System.Threading.Tasks.Task AddAsync_ShouldAddToDo()
    {
        // Arrange
        using var context = new AppDbContext(_dbContextOptions, new AuditInterceptor());
        var repository = new ToDoRepository(context);
        var toDo = new ToDo(DateTime.Now.AddDays(1), new Title("Test Title"), new Description("Test Description"), 50);

        // Act
        var id = await repository.AddAsync(toDo);

        // Assert
        var addedToDo = await context.ToDos.FindAsync(id);
        Assert.NotNull(addedToDo);
        Assert.Equal("Test Title", addedToDo.Title.Value);
        Assert.Equal("Test Description", addedToDo.Description.Value);
        Assert.Equal(50, addedToDo.Complete);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetByIdAsync_ShouldReturnToDo_WhenExists()
    {
        // Arrange
        using var context = new AppDbContext(_dbContextOptions, new AuditInterceptor());
        var repository = new ToDoRepository(context);
        var toDo = new ToDo(DateTime.Now.AddDays(1), new Title("Test Title"), new Description("Test Description"), 50);
        context.ToDos.Add(toDo);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(toDo.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(toDo.Id, result.Id);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetByIdAsync_ShouldThrowNotFoundException_WhenNotExists()
    {
        // Arrange
        using var context = new AppDbContext(_dbContextOptions, new AuditInterceptor());
        var repository = new ToDoRepository(context);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => repository.GetByIdAsync(Guid.NewGuid()));
    }

    [Fact]
    public async System.Threading.Tasks.Task GetAllAsync_ShouldReturnAllToDos()
    {
        // Arrange
        using var context = new AppDbContext(_dbContextOptions, new AuditInterceptor());
        var repository = new ToDoRepository(context);
        context.ToDos.AddRange(
            new ToDo(DateTime.Now.AddDays(1), new Title("Title1"), new Description("Description1"), 50),
            new ToDo(DateTime.Now.AddDays(2), new Title("Title2"), new Description("Description2"), 75)
        );
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async System.Threading.Tasks.Task DeleteAsync_ShouldRemoveToDo()
    {
        // Arrange
        using var context = new AppDbContext(_dbContextOptions, new AuditInterceptor());
        var repository = new ToDoRepository(context);
        var toDo = new ToDo(DateTime.Now.AddDays(1), new Title("Test Title"), new Description("Test Description"), 50);
        context.ToDos.Add(toDo);
        await context.SaveChangesAsync();

        // Act
        await repository.DeleteAsync(toDo.Id);

        // Assert
        var deletedToDo = await context.ToDos.FindAsync(toDo.Id);
        Assert.Null(deletedToDo);
    }

    [Fact]
    public async System.Threading.Tasks.Task UpdateAsync_ShouldUpdateToDo()
    {
        // Arrange
        using var context = new AppDbContext(_dbContextOptions, new AuditInterceptor());
        var repository = new ToDoRepository(context);
        var toDo = new ToDo(DateTime.Now.AddDays(1), new Title("Old Title"), new Description("Old Description"), 50);
        context.ToDos.Add(toDo);
        await context.SaveChangesAsync();

        // Act
        toDo.UpdateTitle(new Title("New Title"));
        toDo.UpdateDescription(new Description("New Description"));
        toDo.UpdateComplete(75);
        await repository.UpdateAsync(toDo);

        // Assert
        var updatedToDo = await context.ToDos.FindAsync(toDo.Id);
        Assert.NotNull(updatedToDo);
        Assert.Equal("New Title", updatedToDo.Title.Value);
        Assert.Equal("New Description", updatedToDo.Description.Value);
        Assert.Equal(75, updatedToDo.Complete);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetIncomingToDosAsync_ShouldReturnFilteredToDos()
    {
        // Arrange
        using var context = new AppDbContext(_dbContextOptions, new AuditInterceptor());
        var repository = new ToDoRepository(context);
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);
        var endOfWeek = today.AddDays(7 - (int)today.DayOfWeek);

        context.ToDos.AddRange(
            new ToDo(today, new Title("Today"), new Description("Today Description"), 50),
            new ToDo(tomorrow, new Title("Tomorrow"), new Description("Tomorrow Description"), 75),
            new ToDo(endOfWeek, new Title("End of Week"), new Description("End of Week Description"), 100),
            new ToDo(endOfWeek.AddDays(1), new Title("Next Week"), new Description("Next Week Description"), 25)
        );
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetIncomingToDosAsync(today, tomorrow, endOfWeek);

        // Assert
        Assert.Equal(3, result.Count());
    }
}
