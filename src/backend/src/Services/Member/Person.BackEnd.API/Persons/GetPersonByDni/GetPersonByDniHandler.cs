namespace Person.BackEnd.API.Persons.GetPersonByDni;

public record GetPersonByDniQuery(string Dni) : IQuery<GetPersonByDniResult>;

public record GetPersonByDniResult(Models.Person Person);

internal class GetCustomerByDniQueryHandler(IDocumentSession session, IServiceLogger logger)
    : IQueryHandler<GetPersonByDniQuery, GetPersonByDniResult>
{
    public async Task<GetPersonByDniResult> Handle(GetPersonByDniQuery query, CancellationToken cancellationToken)
    {
        var customer = session
            .Query<Models.Person>()
            .FirstOrDefault(p => p.Dni == query.Dni);

        if (customer is null)
            throw new PersonNotFoundException(query.Dni);

        return new GetPersonByDniResult(customer);
    }
}