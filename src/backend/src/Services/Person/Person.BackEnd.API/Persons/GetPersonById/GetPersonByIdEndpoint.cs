namespace Person.BackEnd.API.Persons.GetPersonById;

//public record GetPersonByIdRequest();

public record GetPersonByIdResponse(Models.Person Person);

public class GetPersonByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/persons/{id}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new GetPersonByIdQuery(id));

                var response = result.Adapt<GetPersonByIdResponse>();

                return Results.Ok(response);
            })
            .WithName("GetPersonById")
            .Produces<GetPersonByIdResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Person By Id")
            .WithDescription("Get Person By Id");
    }
}