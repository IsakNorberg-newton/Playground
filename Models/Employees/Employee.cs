using System.Collections.Immutable;
using Seido.Utilities.SeedGenerator;

namespace Models.Employees;

public enum WorkRole {AnimalCare, Veterinarian, ProgramCoordinator, Maintenance, Management}
public record Employee (
    Guid EmployeeId,
    string FirstName,
    string LastName,
    DateTime HireDate,
    WorkRole Role,
    ImmutableList<CreditCard> CreditCards,
    bool Seeded = false
) : ISeed<Employee>
{
    public Employee() : this(default, default, default, default, default, default) {}

    #region randomly seed this instance
    public virtual Employee Seed (SeedGenerator seeder)
    {
        //generate 0 to 3 credit cards
        var cards = seeder.ItemsToList<CreditCard>(seeder.Next(0, 4));

        var ret = new Employee(
            Guid.NewGuid(),
            seeder.FirstName,
            seeder.LastName,
            seeder.DateAndTime(2000, 2024),
            seeder.FromEnum<WorkRole>(),
            cards.ToImmutableList(),
            true
        );
        return ret;
    }
    #endregion
    
}

