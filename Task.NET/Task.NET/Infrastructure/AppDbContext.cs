using Microsoft.EntityFrameworkCore;
using Task.NET.Domain.Entities;
using Task.NET.Shared.Interceptors;

namespace Task.NET.Infrastructure;

public class AppDbContext : DbContext
{
    private readonly AuditInterceptor _auditInterceptor;
    public AppDbContext(
        DbContextOptions<AppDbContext> options,
        AuditInterceptor auditInterceptor) : base(options)
    {
        _auditInterceptor = auditInterceptor;
    }

    public virtual DbSet<ToDo> ToDos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.AddInterceptors(_auditInterceptor);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
