namespace Person.BackEnd.API.Persons.UpdateMember;

public record UpdatePersonCommand(
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
    Models.Membership Membership) : ICommand<UpdatePersonResult>;

public record UpdatePersonResult(bool IsSuccess);

public class UpdatePersonCommandValidator : AbstractValidator<UpdatePersonCommand>
{
    public UpdatePersonCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty()
            .WithMessage("El campo ID es requerido");
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

public class UpdatePersonHandler(IDocumentSession session)
    : ICommandHandler<UpdatePersonCommand, UpdatePersonResult>
{
    public async Task<UpdatePersonResult> Handle(UpdatePersonCommand command, CancellationToken cancellationToken)
    {
        var person = await session.LoadAsync<Models.Person>(command.Id, cancellationToken);

        if (person is null) throw new PersonNotFoundException(command.Id);

        person.Dni = command.Dni;
        person.FirstName = command.FirstName;
        person.FirstSurname = command.FirstSurname;
        person.SecondSurname = command.SecondSurname;
        person.Gender = command.gender;
        person.Birthdate = command.Birthdate;
        person.MaritalStatus = command.MaritalStatus;
        person.Nationality = command.Nationality;
        person.PhoneNumber = command.PhoneNumber;
        person.Mobile = command.Mobile;
        person.Email = command.Email;
        person.IsBaptized = command.IsBaptized;
        person.Skills = command.Skills;
        person.Address = command.Address;
        person.EmergencyContact = command.EmergencyContact;
        person.Labor = command.Labor;
        person.PersonalHealthStatus = command.PersonalHealthStatus;
        person.Membership = command.Membership;

        session.Update(person);
        await session.SaveChangesAsync(cancellationToken);

        return new UpdatePersonResult(true);
    }
}