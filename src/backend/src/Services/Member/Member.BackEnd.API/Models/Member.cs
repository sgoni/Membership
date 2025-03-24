namespace Member.BackEnd.API.Models;

public class Member
{
    public Guid Id { get; set; }
    public string Dni { get; set; }
    public string? FirstName { get; set; }
    public string? FirstSurname { get; set; }
    public string? SecondSurname { get; set; }
    public char? Gender { get; set; }
    public DateTime Birthdate { get; set; }
    public int Age { get; set; }
    public string? MaritalStatus { get; set; }
    public string? Nationality { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Mobile { get; set; }
    public string? Email { get; set; }
    public bool IsBaptized { get; set; }
    public string? Skills { get; set; }
    public Address? Address { get; set; }
    public EmergencyContact? EmergencyContact { get; set; }
    public Labor? Labor { get; set; }
    public PersonalHealthStatus? PersonalHealthStatus { get; set; }
    public Membership? Membership { get; set; }

    public void CalculateAge()
    {
        var today = DateTime.Today;
        Age = today.Year - Birthdate.Year;

        // Ajustar la edad si aún no ha pasado el cumpleaños este año
        if (Birthdate.Date > today.AddYears(-Age))
        {
            Age--;
        }
    }
}

public record Address
{
    public int Province { get; set; }
    public int Canton { get; set; }
    public int District { get; set; }
    public string? Street { get; set; }
    public string? ZipCode { get; set; }
    public double? Longitude { get; set; }
    public double? Latitude { get; set; }
    public string PlusCode { get; set; }
}

public record EmergencyContact
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
}

public record Labor
{
    public string? Profession { get; set; }
    public double? LaborSector { get; set; }
}

public record PersonalHealthStatus
{
    public bool MedicalCondition { get; set; }
    public string Description { get; set; }
}

public record Membership
{
    public bool MembershipStatus { get; set; }
    public int TimeToCongregate { get; set; }
    public string Area { get; set; }
    public string Attendance { get; set; }
}