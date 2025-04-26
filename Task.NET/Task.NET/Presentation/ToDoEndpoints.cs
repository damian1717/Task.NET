using MediatR;
using Microsoft.AspNetCore.Mvc;
using Task.NET.Application.Commands.CreateToDo;
using Task.NET.Application.Commands.DeleteToDo;
using Task.NET.Application.Commands.MarkToDoAsDone;
using Task.NET.Application.Commands.SetToDoPercentComplate;
using Task.NET.Application.Commands.UpdateToDo;
using Task.NET.Application.Queries.GetAllToDos;
using Task.NET.Application.Queries.GetIncomingToDos;
using Task.NET.Application.Queries.GetToDoById;

namespace Task.NET.Presentation;

public class ToDoEndpoints
{
    public static void MapToDoEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/todos",
            async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetAllToDosQuery());

                if (result.IsSuccess)
                {
                    return Results.Ok(result.Value);
                }

                return Results.Problem(result.Error);
            })
            .WithName("GetAllToDos");

        app.MapGet("/todos/{id:guid}",
            async ([FromRoute] Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetToDoByIdQuery(id));

                if (result.IsSuccess)
                {
                    return Results.Ok(result.Value);
                }

                return Results.NotFound(result.Error);
            })
            .WithName("GetToDoById");

        app.MapGet("/todos/incoming", 
            async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetIncomingToDosQuery());

                if (result.IsSuccess)
                {
                    return Results.Ok(result.Value);
                }

                return Results.BadRequest(result.Error);
            })
            .WithName("GetIncomingToDos");

        app.MapPost("/todos",
            async ([FromBody] CreateToDoCommand command, IMediator mediator) =>
            {
                var result = await mediator.Send(command);
                if (result.IsSuccess)
                {
                    return Results.Created($"/todos/{result.Value}", result.Value);
                }
                return Results.BadRequest(result.Error);
            })
            .WithName("CreateToDo");

        app.MapPatch("/todos/{id}",
            async (Guid id, [FromBody] UpdateToDoCommand command, IMediator mediator) =>
            {
                if (id != command.Id)
                {
                    return Results.BadRequest("ID mismatch between URL and payload.");
                }

                var result = await mediator.Send(command);

                if (result.IsSuccess)
                {
                    return Results.Ok(result.Value);
                }

                return Results.BadRequest(result.Error);
            })
            .WithName("UpdateToDo");

        app.MapPatch("/todos/{id}/percent",
            async (Guid id, [FromBody] int percentComplete, IMediator mediator) =>
            {
                var command = new SetToDoPercentCompleteCommand(id, percentComplete);
                var result = await mediator.Send(command);

                if (result.IsSuccess)
                {
                    return Results.NoContent();
                }

                return Results.BadRequest(result.Error);
            })
            .WithName("SetToDoPercentComplete");

        app.MapDelete("/todos/{id:guid}",
            async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new DeleteToDoCommand(id));
                if (result.IsSuccess)
                {
                    return Results.NoContent();
                }
                return Results.BadRequest(result.Error);
            })
            .WithName("DeleteToDo");

        app.MapPatch("/todos/{id}/done",
            async (Guid id, IMediator mediator) =>
            {
                var command = new MarkToDoAsDoneCommand(id);
                var result = await mediator.Send(command);

                if (result.IsSuccess)
                {
                    return Results.NoContent();
                }

                return Results.BadRequest(result.Error);
            })
            .WithName("MarkToDoAsDone");
    }
}
