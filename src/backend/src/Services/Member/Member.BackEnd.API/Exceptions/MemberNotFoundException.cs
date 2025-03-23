namespace Member.BackEnd.API.Exceptions;

public class CustomerNotFoundException : NotFoundException
{
    public CustomerNotFoundException(object key) : base("customer", key)
    {
    }
}