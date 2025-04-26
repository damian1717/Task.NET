using Task.NET.Application.Dtos;
using Task.NET.Domain.Repositories;
using Task.NET.Shared.Application;
using Task.NET.Shared.Entities;

namespace Task.NET.Application.Queries.GetAllToDos;

public class GetAllToDosHandler : BaseRequestHandler<GetAllToDosQuery, List<ToDoDto>>
{
    private readonly IToDoRepository _toDoRepository;
    public GetAllToDosHandler(
        ILogger<GetAllToDosHandler> logger,
        IToDoRepository toDoRepository) : base(logger) => _toDoRepository = toDoRepository;

    public override async Task<Result<List<ToDoDto>>> HandleAsync(
        GetAllToDosQuery request,
        CancellationToken cancellationToken)
    {
        var toDos = await _toDoRepository.GetAllAsync();

        var todoDtos = toDos.Select(x =>
            new ToDoDto(
                x.Id,
                x.DateTimeOfExpiry,
                x.Title.Value,
                x.Description.Value,
                x.Complete)).ToList();

        return Result<List<ToDoDto>>.Success(todoDtos);
    }
}
