namespace Person.BackEnd.API.Persons.DeletePerson;

public record DeletePersonCommand(Guid id) : ICommand<DeletePersonResult>;

public record DeletePersonResult(bool IsSuccess);

public class DeletePersonCommandValidator : AbstractValidator<DeletePersonCommand>
{
    public DeletePersonCommandValidator()
    {
        RuleFor(x => x.id).NotEmpty().WithMessage("El campo ID es requerido");
    }
}

internal class DeletePersonHandler(IDocumentSession session)
    : ICommandHandler<DeletePersonCommand, DeletePersonResult>
{
    public async Task<DeletePersonResult> Handle(DeletePersonCommand command, CancellationToken cancellationToken)
    {
        session.Delete<Models.Person>(command.id);
        await session.SaveChangesAsync(cancellationToken);

        return new DeletePersonResult(true);
    }
}