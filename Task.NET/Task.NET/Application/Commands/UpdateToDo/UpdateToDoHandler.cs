using Task.NET.Application.Dtos;
using Task.NET.Domain.Entities;
using Task.NET.Domain.Repositories;
using Task.NET.Shared.Application;
using Task.NET.Shared.Entities;
using Task.NET.Shared.Exceptions;

namespace Task.NET.Application.Commands.UpdateToDo;

public class UpdateToDoHandler : BaseRequestHandler<UpdateToDoCommand, ToDoDto>
{
    private readonly IToDoRepository _toDoRepository;

    public UpdateToDoHandler(
        ILogger<UpdateToDoHandler> logger,
        IToDoRepository toDoRepository) : base(logger) => _toDoRepository = toDoRepository;

    public override async Task<Result<ToDoDto>> HandleAsync(
        UpdateToDoCommand request, 
        CancellationToken cancellationToken)
    {
        var toDo = await _toDoRepository.GetByIdAsync(request.Id);

        if (toDo is null)
        {
            throw new NotFoundException(nameof(ToDo), request.Id);
        }

        UpdateToDo(request, toDo);

        await _toDoRepository.UpdateAsync(toDo);

        var toDoDto = new ToDoDto(
            toDo.Id,
            toDo.DateTimeOfExpiry,
            toDo.Title.Value,
            toDo.Description.Value,
            toDo.Complete);

        return Result<ToDoDto>.Success(toDoDto);
    }

    private void UpdateToDo(
        UpdateToDoCommand request,
        ToDo toDo)
    {
        if (request.DateTimeOfExpiry is not null)
            toDo.UpdateDateTimeOfExpiry((DateTime)request.DateTimeOfExpiry);

        if (request.Title is not null)
            toDo.UpdateTitle(new Domain.ValueObjects.ToDo.Title(request.Title));

        if (request.Description is not null)
            toDo.UpdateDescription(new Domain.ValueObjects.ToDo.Description(request.Description));

        if (request.Complete is not null)
            toDo.UpdateComplete((int)request.Complete);
    }
}
