namespace Member.BackEnd.API.Members.CreateMember;

public record CreateMemberCommand(
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
    Models.Membership Membership) : ICommand<CreateMemberResult>;

public record CreateMemberResult(Guid MemberId);

internal class CreateMemberCommandHandler(IDocumentSession session)
    : ICommandHandler<CreateMemberCommand, CreateMemberResult>
{
    public async Task<CreateMemberResult> Handle(CreateMemberCommand command, CancellationToken cancellationToken)
    {
        var member = new Models.Member
        {
            Dni = command.Dni,
            FirstName = command.FirstName,
            FirstSurname = command.FirstSurname,
            SecondSurname = command.SecondSurname,
        };

        //save to database
        session.Store(member);
        await session.SaveChangesAsync(cancellationToken);

        //return result
        return new CreateMemberResult(member.Id);
    }
}