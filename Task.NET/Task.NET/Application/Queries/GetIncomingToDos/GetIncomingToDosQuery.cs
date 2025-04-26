using MediatR;
using Task.NET.Application.Dtos;
using Task.NET.Shared.Entities;

namespace Task.NET.Application.Queries.GetIncomingToDos;

public record GetIncomingToDosQuery
    : IRequest<Result<List<ToDoDto>>>;
