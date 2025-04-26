using Task.NET.Application.Dtos;
using Task.NET.Application.Queries.GetAllToDos;
using Task.NET.Domain.Entities;
using Task.NET.Domain.Repositories;
using Task.NET.Shared.Application;
using Task.NET.Shared.Entities;
using Task.NET.Shared.Exceptions;

namespace Task.NET.Application.Queries.GetToDoById;

public class GetToDoByIdHandler : BaseRequestHandler<GetToDoByIdQuery, ToDoDto>
{
    private readonly IToDoRepository _toDoRepository;
    public GetToDoByIdHandler(
        ILogger<GetToDoByIdHandler> logger,
        IToDoRepository toDoRepository) : base(logger) => _toDoRepository = toDoRepository;

    public override async Task<Result<ToDoDto>> HandleAsync(
        GetToDoByIdQuery request,
        CancellationToken cancellationToken)
    {
        var toDo = await _toDoRepository.GetByIdAsync(request.Id);

        if (toDo is null)
        {
            throw new NotFoundException(nameof(ToDo), request.Id);
        }

        var toDoDto = new ToDoDto(
            toDo.Id,
            toDo.DateTimeOfExpiry,
            toDo.Title.Value,
            toDo.Description.Value,
            toDo.Complete);

        return Result<ToDoDto>.Success(toDoDto);
    }
}
