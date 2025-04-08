namespace Person.BackEnd.API.Data;

public class PersonInitialData : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        using var session = store.LightweightSession();

        if (await session.Query<Models.Person>().AnyAsync()) return;

        // Marten UPSERT will cater for existing records
        session.Store(GetPreconfigureCustomers());
        await session.SaveChangesAsync();
    }

    private static IEnumerable<Models.Person> GetPreconfigureCustomers()
    {
        var rand = new Random();
        var firstNames = new[]
        {
            "Carlos", "Ana", "Luis", "María", "José", "Laura", "Pedro", "Sofía", "Jorge", "Valeria", "Steven",
            "Claudio", "Felipe", "Esteban", "Roberto", "Matias", "Elias", "Miguel", "Santiago", "Sonia", "Alfonzo"
        };
        var surnames = new[]
        {
            "Ramírez", "Mora", "Gómez", "Hernández", "Vargas", "Fernández", "Castro", "Sánchez", "Jiménez", "Rojas",
            "MIranda", "Conejo", "Sandi", "Blanco", "Arguello", "Segura", "Lopez", "Bejarano", "Guttierrez"
        };
        var skillsList = new[]
            { "Teaching", "Singing", "Preaching", "Cooking", "Driving", "Music", "Art", "Programming" };
        var professions = new[]
        {
            "Engineer", "Teacher", "Doctor", "Artist", "Lawyer", "Mechanic", "Bus Driver", "Accountant", "Architect",
            "Psychologist", "Waiter/Waitress", "Baker", "Chemist", "Cobbler", "Dentist", "Factory worker", "Florist",
            "Housewife", "Hairdresser", "Lawyer", "Salesman", "Taxi driver", "Veterinarian", "Shop Assistant",
            "Secretary", "Receptionist", "Programmer", "Plumber", "Optician"
        };
        var laborsSector = new[]
        {
            "Agriculture", "Construction", "Commerce", "Education", "Healthcare", "Manufacturing", "Transportation",
            "Information Technology (IT)", "Finance", "Hospitality / Tourism", "Public Administration",
            "Energy", "Real Estate", "Legal Services", "Telecommunications", "Mining", "Arts and Entertainment",
            "Retail", "Wholesale", "Environmental Services", "Food", "Other"
        };
        var areas = new[]
        {
            "Alabanza y Adoración", "JEME", "Comisión de Tecnología", "Pilares de Bendición", "Diáconos", "Red Florece",
            "Red Transformados", "Aposento Alto", "Escuela Bíblica", "Misiones", "Vida y Familia", "C.E.M",
            "Equipo Pastoral", "Administración"
        };
        var attendances = new[] { "Regular", "Occasional", "Frequent" };
        var genders = new[] { 'M', 'F' };
        var maritalStatuses = new[] { "Single", "Married", "Divorced", "Widowed" };
        var nationalities = new[]
        {
            "Costarricense", "Nicaraguense", "Panamenio", "Estadounidense", "Gualtemalteco", "Salvadorenio",
            "Venezolano"
        };

        var people = new List<Models.Person>();

        for (var i = 0; i < 50; i++)
        {
            var firstName = firstNames[rand.Next(firstNames.Length)];
            var firstSurname = surnames[rand.Next(surnames.Length)];
            var secondSurname = surnames[rand.Next(surnames.Length)];
            var gender = genders[rand.Next(genders.Length)];
            var maritalStatus = maritalStatuses[rand.Next(maritalStatuses.Length)];
            var nationality = nationalities[rand.Next(nationalities.Length)];

            people.Add(new Models.Person
            {
                Dni = rand.Next(10000000, 99999999).ToString(),
                FirstName = firstName,
                FirstSurname = firstSurname,
                SecondSurname = secondSurname,
                Gender = gender,
                Birthdate = DateTime.Now.AddYears(-rand.Next(18, 60)).AddDays(-rand.Next(1, 365)),
                MaritalStatus = maritalStatus,
                Nationality = nationality,
                PhoneNumber = $"22{rand.Next(100000, 999999)}",
                Mobile = $"88{rand.Next(100000, 999999)}",
                Email = $"{firstName.ToLower()}.{firstSurname.ToLower()}@example.com",
                IsBaptized = rand.Next(0, 2) == 1,
                Skills = skillsList[rand.Next(skillsList.Length)],
                Address = new Address
                {
                    Province = rand.Next(1, 8),
                    Canton = rand.Next(1, 10),
                    District = rand.Next(1, 15),
                    Street = $"Calle {rand.Next(1, 100)}",
                    ZipCode = rand.Next(10000, 10999).ToString(),
                    Longitude = rand.NextDouble() * -85.0,
                    Latitude = rand.NextDouble() * 10.0,
                    PlusCode = "WVR4+9H San José"
                },
                EmergencyContact = new EmergencyContact
                {
                    FirstName = firstNames[rand.Next(firstNames.Length)],
                    LastName = surnames[rand.Next(surnames.Length)],
                    PhoneNumber = $"87{rand.Next(100000, 999999)}"
                },
                Labor = new Labor
                {
                    Profession = professions[rand.Next(professions.Length)],
                    LaborSector = laborsSector[rand.Next(laborsSector.Length)]
                },
                PersonalHealthStatus = new PersonalHealthStatus
                {
                    MedicalCondition = rand.Next(0, 2) == 1,
                    Description = "No issues" // puedes randomizar también esto
                },
                Membership = new Models.Membership
                {
                    MembershipStatus = rand.Next(0, 2) == 1,
                    TimeToCongregate = rand.Next(1, 10),
                    Area = areas[rand.Next(areas.Length)],
                    Attendance = attendances[rand.Next(attendances.Length)]
                }
            });
        }

        return people;
    }
}