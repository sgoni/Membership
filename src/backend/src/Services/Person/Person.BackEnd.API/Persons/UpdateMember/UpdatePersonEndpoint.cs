namespace Person.BackEnd.API.Persons.UpdateMember;

public record UpdatePersonRequest(
    Guid Id,
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

public record UpdatePersonResponse(bool IsSuccess);

public class UpdatePersonEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/persons", async (UpdatePersonRequest request, ISender sender) =>
            {
                var command = request.Adapt<UpdatePersonCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<UpdatePersonResponse>();

                return Results.Ok(response);
            })
            .WithName("UpdatePerson")
            .Produces<UpdatePersonResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Person")
            .WithDescription("Update Person");
    }
}