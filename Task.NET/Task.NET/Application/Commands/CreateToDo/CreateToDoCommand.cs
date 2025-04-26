using MediatR;
using Task.NET.Shared.Entities;

namespace Task.NET.Application.Commands.CreateToDo;

public record CreateToDoCommand(
    DateTime DateTimeOfExpiry,
    string Title,
    string Description, 
    int Complete) : IRequest<Result<Guid>>;
