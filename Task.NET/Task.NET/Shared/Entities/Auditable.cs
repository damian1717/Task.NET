using Task.NET.Shared.Entities.Abstraction;

namespace Task.NET.Shared.Entities;

public class Auditable<TKey> : Entity<TKey>, IAuditable
{
    public int? InsertedUserId { get; private set; }
    public DateTime? InsertedTime { get; private set; }
    public int? UpdatedUserId { get; private set; }
    public DateTime? UpdatedTime { get; private set; }
}
