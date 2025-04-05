namespace Person.BackEnd.API.Persons.GetPersons;

public record GetPersonsQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetPersonsResult>;

public record GetPersonsResult(IEnumerable<Models.Person> Persons);

public class GetPersonQueryHandler(IDocumentSession session) : IQueryHandler<GetPersonsQuery, GetPersonsResult>
{
    public async Task<GetPersonsResult> Handle(GetPersonsQuery query, CancellationToken cancellationToken)
    {
        var persons = await session.Query<Models.Person>()
            .ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10, cancellationToken);
        return new GetPersonsResult(persons);
    }
}