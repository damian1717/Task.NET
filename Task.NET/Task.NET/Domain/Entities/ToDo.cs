using Task.NET.Domain.ValueObjects.ToDo;
using Task.NET.Shared.Entities;
using Task.NET.Shared.Exceptions;

namespace Task.NET.Domain.Entities;

public class ToDo : Auditable<Guid>
{
    public DateTime DateTimeOfExpiry { get; private set; }
    public Title Title { get; private set; }
    public Description Description { get; private set; }
    public int Complete { get; private set; }

    public ToDo()
    {
        
    }

    public ToDo(
        DateTime dateTimeOfExpiry,
        Title title,
        Description description,
        int complete)
    {

        DateTimeOfExpiry = dateTimeOfExpiry;
        Title = title;
        Description = description;
        Complete = complete;
    }

    public static ToDo Create(
        DateTime dateTimeOfExpiry,
        Title title,
        Description description,
        int complete)
    {

        ValidateComplete(complete);

        return new ToDo(
            dateTimeOfExpiry,
            title,
            description,
            complete);
    }

    public void UpdateDateTimeOfExpiry(DateTime dateTimeOfExpiry)
    {
        DateTimeOfExpiry = dateTimeOfExpiry;
    }

    public void UpdateTitle(Title title)
    {
        Title = title;
    }

    public void UpdateDescription(Description description)
    {
        Description = description;
    }

    public void UpdateComplete(int complete)
    {
        ValidateComplete(complete);

        Complete = complete;
    }

    private static void ValidateComplete(int complete)
    {
        if (complete < 0 || complete > 100)
        {
            throw new DomainException("Complete must be between 0 and 100.");
        }
    }
}
