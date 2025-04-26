namespace Task.NET.Application.Dtos;

public record ToDoDto(
    Guid Id,
    DateTime DateTimeOfExpiry,
    string Title,
    string Description,
    int Complete);
