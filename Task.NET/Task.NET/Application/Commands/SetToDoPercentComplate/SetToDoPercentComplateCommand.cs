using MediatR;
using Task.NET.Shared.Entities;

namespace Task.NET.Application.Commands.SetToDoPercentComplate;

public record SetToDoPercentCompleteCommand(Guid Id, int PercentComplete) 
    : IRequest<Result<Guid>>;
