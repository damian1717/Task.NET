using MediatR;
using Task.NET.Application.Dtos;
using Task.NET.Shared.Entities;

namespace Task.NET.Application.Queries.GetToDoById;

public record GetToDoByIdQuery(Guid Id) 
    : IRequest<Result<ToDoDto>>;