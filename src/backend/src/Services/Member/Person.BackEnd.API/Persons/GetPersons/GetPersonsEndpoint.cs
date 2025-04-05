namespace Person.BackEnd.API.Persons.GetPersons;

public record GetPersonsRequest(int? PageNumber = 1, int? PageSize = 10);

public record GetPersonsResponse(IEnumerable<Models.Person> Persons);

public class GetPersonsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/persons", async ([AsParameters] GetPersonsRequest request, ISender sender) =>
            {
                var query = request.Adapt<GetPersonsQuery>();

                var result = await sender.Send(query);

                var response = result.Adapt<GetPersonsResponse>();

                return Results.Ok(response);
            })
            .WithName("GetPersons")
            .Produces<GetPersonsResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Persons")
            .WithDescription("Get Persons");
    }
}