using System.Linq;
using Models.Employees;

namespace Playground.Lesson02;

public static class LinqQuantifier
{
    public static void RunExamples(IEnumerable<Employee> employees)
    {
        System.Console.WriteLine("\nLinq Quantifier Examples:");
        System.Console.WriteLine("--------------------------------");

        // Materialize to avoid multiple enumeration and to use reference-equality checks
        var list = employees.ToList();

        // All: are all employees having a non-empty first name?
        var allHaveFirstName = list.All(e => !string.IsNullOrWhiteSpace(e.FirstName));
        System.Console.WriteLine($"All have first names: {allHaveFirstName}");

        // Any: is there at least one veterinarian?
        var anyVeterinarians = list.Any(e => e.Role == WorkRole.Veterinarian);
        System.Console.WriteLine($"Any veterinarians: {anyVeterinarians}");

        // Contains: check membership by record equality (records implement value equality)
        var sample = list.FirstOrDefault();
        if (sample is not null)
        {
            var containsSample = list.Contains(sample);
            System.Console.WriteLine($"Contains first employee (by record equality): {containsSample}");
        }
        else
        {
            System.Console.WriteLine("No employees available to demonstrate Contains.");
        }

        // SequenceEqual: compare two sequences for same elements in same order
        var firstThree = list.Take(3).ToList();
        var sameOrder = new List<Employee>(firstThree);
        var differentOrder = firstThree.AsEnumerable().Reverse().ToList();
        System.Console.WriteLine($"SequenceEqual (same order): {firstThree.SequenceEqual(sameOrder)}");
        System.Console.WriteLine($"SequenceEqual (different order): {firstThree.SequenceEqual(differentOrder)}");

    }
}
