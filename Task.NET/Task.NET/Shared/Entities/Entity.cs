using Task.NET.Shared.Entities.Abstraction;

namespace Task.NET.Shared.Entities;

public abstract class Entity<T> : IEntity<T>
{
    public T Id { get; private set; }
}
