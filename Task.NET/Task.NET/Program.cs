using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Task.NET.Infrastructure;
using Task.NET.Infrastructure.DI;
using Task.NET.Presentation;
using Task.NET.Shared.Provider;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddMediatR(delegate (MediatRServiceConfiguration cfg)
{
    cfg.RegisterServicesFromAssemblies(AssembliesProvider.Get(["Task.NET"]).ToArray());
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Task.NET API",
        Version = "v1",
        Description = "API ToDos"
    });
});

builder.Services.AddInfrastructure();

var app = builder.Build();

//app.ApplyMigrations();

app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Task.NET API v1");
    options.RoutePrefix = "swagger";
});

ToDoEndpoints.MapToDoEndpoints(app);

app.Run();
