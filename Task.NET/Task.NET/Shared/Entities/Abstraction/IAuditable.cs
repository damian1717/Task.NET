namespace Task.NET.Shared.Entities.Abstraction;

public interface IAuditable
{
    public int? InsertedUserId { get; }
    public DateTime? InsertedTime { get; }
    public int? UpdatedUserId { get; }
    public DateTime? UpdatedTime { get; }
}
