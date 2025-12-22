using System.Linq;
using Models.Employees;

namespace Playground.Lesson02;

public static class LinqOrdering
{
    public static void RunExamples(IEnumerable<Employee> employees)
    {
        System.Console.WriteLine("\nLinq Ordering Examples:");
        System.Console.WriteLine("--------------------------------");

        // Ordering examples
        // OrderBy: by LastName then FirstName
        var byName = employees.OrderBy(e => e.LastName).ThenBy(e => e.FirstName);
        System.Console.WriteLine("Ordered by LastName, ThenBy FirstName (showing 10):");
        byName.Take(10).ToList().ForEach(e => System.Console.WriteLine($"{e.LastName}, {e.FirstName} - {e.Role}"));

        System.Console.WriteLine();

        // OrderByDescending and ThenByDescending
        var byHireDesc = employees.OrderByDescending(e => e.HireDate).ThenByDescending(e => e.LastName);
        System.Console.WriteLine("Ordered by HireDate desc, ThenByDescending LastName (showing 10):");
        byHireDesc.Take(10).ToList().ForEach(e => System.Console.WriteLine($"{e.HireDate:d} - {e.FirstName} {e.LastName}"));

        System.Console.WriteLine();

        // Reverse: reverse the current sequence (after ordering)
        var reversed = byName.Take(10).Reverse();
        System.Console.WriteLine("Reversed first 10 from previous ordering:");
        reversed.ToList().ForEach(e => System.Console.WriteLine($"{e.LastName}, {e.FirstName}"));
    }
}
