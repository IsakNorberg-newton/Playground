using System.Linq;
using Models.Employees;

namespace Playground.Lesson02;

public static class LinqGrouping
{
    public static void RunExamples(IEnumerable<Employee> employees)
    {
        System.Console.WriteLine("\nLinq Grouping Examples:");
        System.Console.WriteLine("--------------------------------");

        // GroupBy: group employees by Role and show counts + sample names
        var byRole = employees.GroupBy(e => e.Role)
            .Select(g => new { Role = g.Key, Count = g.Count(), Sample = g.Take(5).ToList() });

        System.Console.WriteLine("Employees grouped by Role:");
        foreach (var grp in byRole)
        {
            System.Console.WriteLine($"{grp.Role} - {grp.Count} employees (showing up to 5):");
            foreach (var emp in grp.Sample)
                System.Console.WriteLine($"  {emp.FirstName} {emp.LastName} - Hired: {emp.HireDate:d}");
        }

        System.Console.WriteLine();

        // GroupBy with ordering: group by year of HireDate
        var byHireYear = employees.GroupBy(e => e.HireDate.Year)
            .OrderBy(g => g.Key)
            .Select(g => new { Year = g.Key, Count = g.Count(), Names = g.Select(e => $"{e.FirstName} {e.LastName}").Take(3) });

        System.Console.WriteLine("Employees grouped by Hire Year (showing first 3 names):");
        foreach (var y in byHireYear.Take(10))
        {
            System.Console.WriteLine($"{y.Year}: {y.Count} employees");
            foreach (var n in y.Names)
                System.Console.WriteLine($"  {n}");
        }

        System.Console.WriteLine();

        // ToLookup: fast lookup by Role (useful for repeated queries)
        var lookupByRole = employees.ToLookup(e => e.Role);
        System.Console.WriteLine("ToLookup by Role:");
        foreach (var roleGroup in lookupByRole)
        {
            System.Console.WriteLine($"{roleGroup.Key} - {roleGroup.Count()} employees (showing up to 3):");
            foreach (var emp in roleGroup.Take(3))
                System.Console.WriteLine($"  {emp.FirstName} {emp.LastName} - Hired: {emp.HireDate:d}");
        }
    }
}
