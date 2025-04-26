namespace Task.NET.Shared.Entities.Abstraction;

public interface IEntity<out TId>
{
    TId Id { get; }
}
