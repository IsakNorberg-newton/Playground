using System.Linq;
using Models.Employees;

namespace Playground.Lesson02;

public static class LinqSetOps
{
    public static void RunExamples(IEnumerable<Employee> employees)
    {
        System.Console.WriteLine("\nLinq Set Operations Examples:");
        System.Console.WriteLine("--------------------------------");

        // Set operations examples
        // Create two sample sets of employees (first 20 and employees with role Veterinarian)
        var first20 = employees.Take(20).ToList();
        var vets = employees.Where(e => e.Role == WorkRole.Veterinarian).ToList();

        // Concat: append sequences (may contain duplicates)
        var concat = first20.Concat(vets);
        System.Console.WriteLine("Concat first20 + vets (showing 10):");
        concat.Take(10).ToList().ForEach(e => System.Console.WriteLine($"{e.FirstName} {e.LastName} - {e.Role}"));

        System.Console.WriteLine();

        // Union: distinct union by reference equality; project to Ids for value-based union
        var unionById = first20.Select(e => e.EmployeeId).Union(vets.Select(e => e.EmployeeId));
        System.Console.WriteLine($"Union (by EmployeeId) count: {unionById.Count()}");

        System.Console.WriteLine();

        // Intersect: employees present in both sets (by EmployeeId)
        var intersectById = first20.Select(e => e.EmployeeId).Intersect(vets.Select(e => e.EmployeeId));
        System.Console.WriteLine($"Intersect (by EmployeeId) count: {intersectById.Count()}");

        System.Console.WriteLine();

        // Except: employees in first20 but not in vets (by EmployeeId)
        var exceptById = first20.Select(e => e.EmployeeId).Except(vets.Select(e => e.EmployeeId));
        System.Console.WriteLine($"Except (first20 \u2260 vets) count: {exceptById.Count()}");

        System.Console.WriteLine();

        // Distinct: remove duplicates after concat by EmployeeId
        var distinctConcat = concat.GroupBy(e => e.EmployeeId).Select(g => g.First());
        System.Console.WriteLine($"Distinct after concat (by EmployeeId) count: {distinctConcat.Count()}");
    }
}
