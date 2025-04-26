using Microsoft.EntityFrameworkCore;
using Task.NET.Domain.Entities;
using Task.NET.Domain.Repositories;
using Task.NET.Shared.Exceptions;

namespace Task.NET.Infrastructure.Repositories;

public class ToDoRepository : IToDoRepository
{
    private readonly AppDbContext _context;

    public ToDoRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Guid> AddAsync(ToDo toDo)
    {
        await _context.ToDos.AddAsync(toDo);
        await _context.SaveChangesAsync();

        return toDo.Id;
    }

    public async System.Threading.Tasks.Task DeleteAsync(Guid id)
    {
        var toDo = await _context.ToDos.FindAsync(id);
        if (toDo is not null)
        {
            _context.ToDos.Remove(toDo);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<ToDo>> GetAllAsync()
        => await _context.ToDos.ToListAsync();

    public async Task<ToDo> GetByIdAsync(Guid id)
    { 
        var toDo = await _context.ToDos.FindAsync(id);

        if (toDo is null)
        {
            throw new NotFoundException(nameof(ToDo), id);
        }

        return toDo;
    }

    public async Task<IEnumerable<ToDo>> GetIncomingToDosAsync(DateTime today, DateTime tomorrow, DateTime endOfWeek)
    {
        return await _context.ToDos
            .Where(x => x.DateTimeOfExpiry.Date == today
                     || x.DateTimeOfExpiry.Date == tomorrow
                     || (x.DateTimeOfExpiry.Date > tomorrow && x.DateTimeOfExpiry.Date <= endOfWeek))
            .ToListAsync();
    }

    public async System.Threading.Tasks.Task UpdateAsync(ToDo toDo)
    {
        _context.ToDos.Update(toDo);
        await _context.SaveChangesAsync();
    }
}
