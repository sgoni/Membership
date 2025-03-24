
namespace Member.BackEnd.API.Members.CreateMember;

public record CreateMemberRequest(
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

public record CreateMemberResponse(Guid MemberId);

public class CreateMemberEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/members", async (CreateMemberRequest request, ISender sender) =>
                {
                    var command = request.Adapt<CreateMemberCommand>();

                    var result = await sender.Send(command);

                    var response = result.Adapt<CreateMemberResponse>();

                    return Results.Created($"/members/{response.MemberId}", response);
                }
            )
            .WithName("CreateMember")
            .Produces<CreateMemberResponse>()
            .ProducesProblem(400)
            .WithSummary("Create Member")
            .WithDescription("Creates a new member.");
    }
}