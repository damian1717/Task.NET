using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Task.NET.Shared.Entities.Abstraction;

namespace Task.NET.Shared.Interceptors;

public class AuditInterceptor : SaveChangesInterceptor
{

    public AuditInterceptor()
    {
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context is null)
        {
            return base.SavingChanges(eventData, result);
        }

        UpdateAuditableData(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        if (eventData.Context is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        UpdateAuditableData(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateAuditableData(DbContext dbContext)
    {
        var utcNow = DateTime.UtcNow;
        var auditEntities = dbContext.ChangeTracker.Entries<IAuditable>();

        foreach (var entry in auditEntities)
        {
            if (entry.State == EntityState.Added)
            {
                SetCurrentPropertyValue(entry, nameof(IAuditable.InsertedTime), utcNow);
                SetCurrentPropertyValue(entry, nameof(IAuditable.InsertedUserId), 1); // for testing userId = 1
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Property(x => x.InsertedTime).IsModified = false;
                entry.Property(x => x.InsertedUserId).IsModified = false;

                SetCurrentPropertyValue(entry, nameof(IAuditable.UpdatedTime), utcNow);
                SetCurrentPropertyValue(entry, nameof(IAuditable.UpdatedUserId), 1); // for testing userId = 1
            }
        }

        static void SetCurrentPropertyValue(
        EntityEntry entry,
        string propertyName,
        object value) =>
        entry.Property(propertyName).CurrentValue = value;
    }
}