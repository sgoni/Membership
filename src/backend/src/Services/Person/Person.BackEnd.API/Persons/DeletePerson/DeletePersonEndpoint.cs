namespace Person.BackEnd.API.Persons.DeletePerson;

//public record DeletePersonRequest(Guid CustomerId);
public record DeletePersonResponse(bool IsSuccess);

public class DeletePersonEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/persons/{id}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new DeletePersonCommand(id));

                var response = result.Adapt<DeletePersonResponse>();

                return Results.Ok(response);
            })
            .WithName("DeletePerson")
            .Produces<DeletePersonResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Person")
            .WithDescription("Delete person");
    }
}