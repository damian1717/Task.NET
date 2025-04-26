using MediatR;
using Task.NET.Shared.Entities;

namespace Task.NET.Application.Commands.DeleteToDo;

public record DeleteToDoCommand(Guid Id) 
    : IRequest<Result<Guid>>;
