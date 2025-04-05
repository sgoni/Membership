namespace Person.BackEnd.API.Exceptions;

public class PersonNotFoundException : NotFoundException
{
    public PersonNotFoundException(object key) : base("person", key)
    {
    }
}