using Task.NET.Domain.Entities;
using Task.NET.Domain.Repositories;
using Task.NET.Shared.Application;
using Task.NET.Shared.Entities;
using Task.NET.Shared.Exceptions;

namespace Task.NET.Application.Commands.DeleteToDo;

public class DeleteToDoHandler : BaseRequestHandler<DeleteToDoCommand, Guid>
{
    private readonly IToDoRepository _toDoRepository;
    public DeleteToDoHandler(
        ILogger<DeleteToDoHandler> logger,
        IToDoRepository toDoRepository) : base(logger)
    {
        _toDoRepository = toDoRepository;
    }

    public override async Task<Result<Guid>> HandleAsync(DeleteToDoCommand request, CancellationToken cancellationToken)
    {
        var toDo = await _toDoRepository.GetByIdAsync(request.Id);

        if (toDo is null)
        {
            throw new NotFoundException(nameof(ToDo), request.Id);
        }

        await _toDoRepository.DeleteAsync(request.Id);

        return Result<Guid>.Success(request.Id);
    }
}
