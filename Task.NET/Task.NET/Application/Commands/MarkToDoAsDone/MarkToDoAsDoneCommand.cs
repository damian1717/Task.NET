using MediatR;
using Task.NET.Shared.Entities;

namespace Task.NET.Application.Commands.MarkToDoAsDone;

public record MarkToDoAsDoneCommand(Guid Id) 
    : IRequest<Result<Guid>>;