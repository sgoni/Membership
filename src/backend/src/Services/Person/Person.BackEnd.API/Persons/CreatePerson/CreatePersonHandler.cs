namespace Person.BackEnd.API.Persons.CreatePerson;

public record CreatePersonCommand(
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
    Models.Membership Membership) : ICommand<CreatePersonResult>;

public record CreatePersonResult(Guid MemberId);

public class CreatePersonCommandValidator : AbstractValidator<CreatePersonCommand>
{
    public CreatePersonCommandValidator()
    {
        RuleFor(x => x.Dni).NotEmpty().Length(8, 15)
            .WithMessage("El campo DNI es requerido");
        RuleFor(x => x.FirstName).NotEmpty().Length(2, 50)
            .WithMessage("El campo nombre es requerido");
        RuleFor(x => x.FirstSurname).NotEmpty().Length(2, 50)
            .WithMessage("El campo apellido es requerido");
        RuleFor(x => x.SecondSurname).NotEmpty().Length(2, 50)
            .WithMessage("El campo apellido es requerido");
        RuleFor(x => x.PhoneNumber).NotEmpty().Length(8, 12).MaximumLength(12)
            .WithMessage("El campo numero telefónico es requerido");
    }
}

internal class CreatePersonCommandHandler(IDocumentSession session)
    : ICommandHandler<CreatePersonCommand, CreatePersonResult>
{
    public async Task<CreatePersonResult> Handle(CreatePersonCommand command, CancellationToken cancellationToken)
    {
        var person = new Models.Person
        {
            Dni = command.Dni,
            FirstName = command.FirstName,
            FirstSurname = command.FirstSurname,
            SecondSurname = command.SecondSurname,
            Gender = command.gender,
            Birthdate = command.Birthdate,
            MaritalStatus = command.MaritalStatus,
            Nationality = command.Nationality,
            PhoneNumber = command.PhoneNumber,
            Mobile = command.Mobile,
            Email = command.Email,
            IsBaptized = command.IsBaptized,
            Skills = command.Skills,
            Address = command.Address,
            EmergencyContact = command.EmergencyContact,
            Labor = command.Labor,
            PersonalHealthStatus = command.PersonalHealthStatus,
            Membership = command.Membership
        };

        //save to database
        session.Store(person);
        await session.SaveChangesAsync(cancellationToken);

        //return result
        return new CreatePersonResult(person.Id);
    }
}