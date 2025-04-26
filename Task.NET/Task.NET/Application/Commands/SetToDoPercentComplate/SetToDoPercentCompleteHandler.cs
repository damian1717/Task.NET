using Task.NET.Domain.Entities;
using Task.NET.Domain.Repositories;
using Task.NET.Shared.Application;
using Task.NET.Shared.Entities;
using Task.NET.Shared.Exceptions;

namespace Task.NET.Application.Commands.SetToDoPercentComplate;

public class SetToDoPercentCompleteHandler : BaseRequestHandler<SetToDoPercentCompleteCommand, Guid>
{
    private readonly IToDoRepository _toDoRepository;
    public SetToDoPercentCompleteHandler(
        ILogger<SetToDoPercentCompleteHandler> logger,
        IToDoRepository toDoRepository) : base(logger)
    {
        _toDoRepository = toDoRepository;
    }

    public override async Task<Result<Guid>> HandleAsync(
        SetToDoPercentCompleteCommand request,
        CancellationToken cancellationToken)
    {
        var toDo = await _toDoRepository.GetByIdAsync(request.Id);

        if (toDo is null)
        {
            throw new NotFoundException(nameof(ToDo), request.Id);
        }

        toDo.UpdateComplete(request.PercentComplete);

        await _toDoRepository.UpdateAsync(toDo);

        return Result<Guid>.Success(toDo.Id);
    }
}

