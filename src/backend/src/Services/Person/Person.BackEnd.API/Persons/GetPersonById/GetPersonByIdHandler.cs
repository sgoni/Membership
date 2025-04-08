namespace Person.BackEnd.API.Persons.GetPersonById;

public record GetPersonByIdQuery(Guid id) : IQuery<GetPersonByIdResult>;

public record GetPersonByIdResult(Models.Person Person);

public class GetPersonByIdHandler(IDocumentSession session) : IQueryHandler<GetPersonByIdQuery, GetPersonByIdResult>
{
    public async Task<GetPersonByIdResult> Handle(GetPersonByIdQuery query, CancellationToken cancellationToken)
    {
        var person = await session.LoadAsync<Models.Person>(query.id, cancellationToken);

        if (person is null)
            throw new PersonNotFoundException(query.id);

        return new GetPersonByIdResult(person);
    }
}