using Task.NET.Application.Dtos;
using Task.NET.Domain.Repositories;
using Task.NET.Shared.Application;
using Task.NET.Shared.Entities;

namespace Task.NET.Application.Queries.GetIncomingToDos;

public class GetIncomingToDosHandler : BaseRequestHandler<GetIncomingToDosQuery, List<ToDoDto>>
{
    private readonly IToDoRepository _toDoRepository;
    public GetIncomingToDosHandler(
        ILogger<GetIncomingToDosHandler> logger,
        IToDoRepository toDoRepository) : base(logger) => _toDoRepository = toDoRepository;

    public override async Task<Result<List<ToDoDto>>> HandleAsync(
        GetIncomingToDosQuery request,
        CancellationToken cancellationToken)
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);
        var endOfWeek = today.AddDays(7 - (int)today.DayOfWeek);

        var todos = await _toDoRepository.GetIncomingToDosAsync(
            today,
            tomorrow,
            endOfWeek);

        var toDoDos = todos
            .Select(x => new ToDoDto(
                x.Id,
                x.DateTimeOfExpiry,
                x.Title.Value,
                x.Description.Value,
                x.Complete))
            .ToList();

        return Result<List<ToDoDto>>.Success(toDoDos);
    }
}
