using System.Collections.Immutable;
using Seido.Utilities.SeedGenerator;

namespace Models.Employees;

public record TempEmployee : Employee, ISeed<TempEmployee>
{
    public TempEmployee() { }

    public TempEmployee(DateTime FinalDate) : base() { }

    public override TempEmployee Seed(SeedGenerator seeder)
    {
        var emp = new Employee().Seed(seeder);

        return new TempEmployee (emp.HireDate.AddYears(seeder.Next(1, 5)))
        {
            EmployeeId = emp.EmployeeId,
            FirstName = emp.FirstName,
            LastName = emp.LastName,
            HireDate = emp.HireDate,
            Role = emp.Role,
            CreditCards = emp.CreditCards,
            Seeded = true
        };
    }
}
