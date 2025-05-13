namespace Person.BackEnd.API.Persons.CreatePerson;

public record CreatePersonRequest(
    string Dni,
    string FirstName,
    string FirstSurname,
    string SecondSurname,
    char gender,
    DateTime Birthdate,
    string MaritalStatus,
    string Nationality,
    string PhoneNumber,
    string Mobile,
    string Email,
    bool IsBaptized,
    string Skills,
    Address Address,
    EmergencyContact EmergencyContact,
    Labor Labor,
    PersonalHealthStatus PersonalHealthStatus,
    Models.Membership Membership);

public record CreatePersonResponse(Guid MemberId);

public class CreatePersonEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/persons", async (CreatePersonRequest request, ISender sender) =>
                {
                    var command = request.Adapt<CreatePersonCommand>();

                    var result = await sender.Send(command);

                    var response = result.Adapt<CreatePersonResponse>();

                    return Results.Created($"/persons/{response.MemberId}", response);
                }
            )
            .WithName("CreatePerson")
            .Produces<CreatePersonResponse>(statusCode: StatusCodes.Status201Created)
            .ProducesProblem(400)
            .WithSummary("Create Person")
            .WithDescription("Creates a new person.");
    }
}