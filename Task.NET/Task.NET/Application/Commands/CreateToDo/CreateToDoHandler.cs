using Task.NET.Domain.Entities;
using Task.NET.Domain.Repositories;
using Task.NET.Shared.Application;
using Task.NET.Shared.Entities;

namespace Task.NET.Application.Commands.CreateToDo;

public class CreateToDoHandler : BaseRequestHandler<CreateToDoCommand, Guid>
{
    private readonly IToDoRepository _toDoRepository;
    public CreateToDoHandler(
        ILogger<CreateToDoHandler> logger,
        IToDoRepository toDoRepository) : base(logger) => _toDoRepository = toDoRepository;

    public override async Task<Result<Guid>> HandleAsync(
        CreateToDoCommand request, 
        CancellationToken cancellationToken)
    {
        var newToDo = ToDo.Create(
            request.DateTimeOfExpiry,
            new Domain.ValueObjects.ToDo.Title(request.Title),
            new Domain.ValueObjects.ToDo.Description(request.Description),
            request.Complete);

        var createdToDoId = await _toDoRepository.AddAsync(newToDo);

        return Result<Guid>.Success(createdToDoId);
    }
}
