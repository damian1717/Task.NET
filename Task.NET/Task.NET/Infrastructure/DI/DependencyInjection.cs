using Task.NET.Domain.Repositories;
using Task.NET.Infrastructure.Repositories;
using Task.NET.Shared.Interceptors;

namespace Task.NET.Infrastructure.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>();
        services.AddScoped<AuditInterceptor>();
        services.AddScoped<IToDoRepository, ToDoRepository>();

        return services;
    }
}
