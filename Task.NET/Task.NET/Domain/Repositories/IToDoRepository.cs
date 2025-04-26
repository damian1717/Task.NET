using Task.NET.Domain.Entities;

namespace Task.NET.Domain.Repositories;

public interface IToDoRepository
{
    Task<ToDo> GetByIdAsync(Guid id);
    Task<IEnumerable<ToDo>> GetAllAsync();
    Task<IEnumerable<ToDo>> GetIncomingToDosAsync(
        DateTime today, 
        DateTime tomorrow, 
        DateTime endOfWeek);

    Task<Guid> AddAsync(ToDo toDo);
    System.Threading.Tasks.Task UpdateAsync(ToDo toDo);
    System.Threading.Tasks.Task DeleteAsync(Guid id);
}
