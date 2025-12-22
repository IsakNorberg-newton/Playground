using System.Linq;
using Models.Employees;

namespace Playground.Lesson02;

public static class LinqJoining
{
    public static void RunExamples(IEnumerable<Employee> employees)
    {
        System.Console.WriteLine("\nLinq Joining Examples:");
        System.Console.WriteLine("--------------------------------");

        // Join: match employees to role metadata
        var roleInfos = new[] {
            new { Role = WorkRole.AnimalCare, Dept = "Animal Care Dept" },
            new { Role = WorkRole.Veterinarian, Dept = "Vet Services" },
            new { Role = WorkRole.ProgramCoordinator, Dept = "Programs" },
            new { Role = WorkRole.Maintenance, Dept = "Facilities" },
            new { Role = WorkRole.Management, Dept = "Management" }
        };

        var joined = employees.Join(roleInfos,
            e => e.Role,
            r => r.Role,
            (e, r) => new { Employee = $"{e.FirstName} {e.LastName}", e.Role, r.Dept });

        System.Console.WriteLine("Join employees with roleInfos (showing 10):");
        joined.Take(10).ToList().ForEach(j => System.Console.WriteLine($"{j.Employee} - {j.Role} - {j.Dept}"));

        System.Console.WriteLine();

        // GroupJoin: roles with grouped employees
        var grouped = roleInfos.GroupJoin(employees,
            r => r.Role,
            e => e.Role,
            (r, emps) => new { r.Dept, Role = r.Role, Count = emps.Count(), Employees = emps });

        System.Console.WriteLine("GroupJoin by role:");
        foreach (var g in grouped)
        {
            System.Console.WriteLine($"{g.Dept} ({g.Role}) - {g.Count} employees");
            foreach (var emp in g.Employees.Take(3))
                System.Console.WriteLine($"  {emp.FirstName} {emp.LastName}");
        }

        System.Console.WriteLine();

        // Zip: pair employees with a sequential index
        var zipped = employees.Take(10).Zip(Enumerable.Range(1, 100), (e, idx) => new { Index = idx, Name = $"{e.FirstName} {e.LastName}", e.Role });
        System.Console.WriteLine("Zip employees with index:");
        zipped.ToList().ForEach(z => System.Console.WriteLine($"{z.Index}: {z.Name} - {z.Role}"));
    }
}
