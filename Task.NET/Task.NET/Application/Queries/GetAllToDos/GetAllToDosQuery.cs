using MediatR;
using Task.NET.Application.Dtos;
using Task.NET.Shared.Entities;

namespace Task.NET.Application.Queries.GetAllToDos;

public record GetAllToDosQuery 
    : IRequest<Result<List<ToDoDto>>>;
