namespace Task.NET.Shared.Exceptions;

public class NotFoundException : Exception
{
    public string? EntityName { get; }
    public object? Key { get; }

    public NotFoundException(string message)
        : base(message) { }


    public NotFoundException(string entityName, object key)
        : base($"{entityName} with ID {key} not found.")
    {
        EntityName = entityName;
        Key = key;
    }

    public NotFoundException(string entityName, object key, string message)
        : base(message)
    {
        EntityName = entityName;
        Key = key;
    }
}
