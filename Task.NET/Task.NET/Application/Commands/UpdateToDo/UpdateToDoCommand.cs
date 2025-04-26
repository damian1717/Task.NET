using MediatR;
using Task.NET.Application.Dtos;
using Task.NET.Shared.Entities;

namespace Task.NET.Application.Commands.UpdateToDo;

public record UpdateToDoCommand(
    Guid Id,
    DateTime? DateTimeOfExpiry,
    string? Title,
    string? Description,
    int? Complete) : IRequest<Result<ToDoDto>>;
