using System.Linq;
using Models.Employees;

namespace Playground.Lesson02;

public static class LinqElementOps
{
    public static void RunExamples(IEnumerable<Employee> employees)
    {
        System.Console.WriteLine("\nLinq Element Operations Examples:");
        System.Console.WriteLine("--------------------------------");

        // Element operators
        // First / FirstOrDefault
        var first = employees.First();
        var firstOrDefault = employees.FirstOrDefault(e => e.Role == WorkRole.Management);
        System.Console.WriteLine($"First employee: {first.FirstName} {first.LastName}");
        System.Console.WriteLine(firstOrDefault is not null
            ? $"FirstOrDefault (Management): {firstOrDefault.FirstName} {firstOrDefault.LastName}"
            : "FirstOrDefault (Management): <none>");

        System.Console.WriteLine();

        // LastOrDefault (use ordering to make result deterministic)
        var lastHired = employees.OrderBy(e => e.HireDate).LastOrDefault();
        System.Console.WriteLine(lastHired is not null
            ? $"LastOrDefault (by hire date): {lastHired.FirstName} {lastHired.LastName} - {lastHired.HireDate:d}"
            : "LastOrDefault: <none>");

        System.Console.WriteLine();

        // Single / SingleOrDefault
        // Pick an employee id that is known to be unique (first one's id) and demonstrate Single
        // Single throws if not exactly one element found
        var singleId = employees.Select(e => e.EmployeeId).FirstOrDefault();
        var singleEmp = employees.Where(e => e.EmployeeId == singleId).Single();
        System.Console.WriteLine($"Single by EmployeeId (existing): {singleEmp.FirstName} {singleEmp.LastName}");

        // SingleOrDefault for a non-existent id returns null instead of throwing
        var missingId = Guid.NewGuid();
        var singleOrDefault = employees.Where(e => e.EmployeeId == missingId).SingleOrDefault();
        System.Console.WriteLine(singleOrDefault is null ? "SingleOrDefault (missing): <null>" : "SingleOrDefault returned a value");

        System.Console.WriteLine();

        // ElementAt / ElementAtOrDefault
        // ElementAt uses zero-based index and throws if index is out of range
        try
        {
            var third = employees.ElementAt(2); // third employee (index 2)
            System.Console.WriteLine($"ElementAt(2): {third.FirstName} {third.LastName}");
        }
        catch (System.InvalidOperationException)
        {
            System.Console.WriteLine("ElementAt(2): sequence has fewer than 3 elements");
        }

        // ElementAtOrDefault returns default(T) (null for reference types) when out of range
        var outOfRange = employees.ElementAtOrDefault(1000);
        System.Console.WriteLine(outOfRange is not null
            ? $"ElementAtOrDefault(1000): {outOfRange.FirstName} {outOfRange.LastName}"
            : "ElementAtOrDefault(1000): <null>");

        System.Console.WriteLine();
        
        // DefaultIfEmpty: provide default when sequence is empty
        var emptySeq = employees.Where(e => e.EmployeeId == missingId).Select(e => e.FirstName);

        // returns the original sequence if it contains any elements; otherwise it returns a single-element sequence containing default(T).
        var firstOrFallback = emptySeq.DefaultIfEmpty("<no employees>").First();
        System.Console.WriteLine($"DefaultIfEmpty -> First(): {firstOrFallback}");


    }
}
