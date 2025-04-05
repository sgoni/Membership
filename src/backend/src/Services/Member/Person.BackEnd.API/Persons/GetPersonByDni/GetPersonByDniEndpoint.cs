namespace Person.BackEnd.API.Persons.GetPersonByDni;

//public record GetCustomerByDniRequest();
public record GetPersonByDniResponse(Models.Person Person);

public class GetPersonByDniEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/persons/dni/{dni}", async (string dni, ISender sender, IServiceLogger logger) =>
            {
                var result = await sender.Send(new GetPersonByDniQuery(dni));

                var response = result.Adapt<GetPersonByDniResponse>();

                return Results.Ok(response);
            })
            .WithName("GetPersonByDni")
            .Produces<GetPersonByDniResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Person By Dni")
            .WithDescription("Get Person By Dni");
    }
}