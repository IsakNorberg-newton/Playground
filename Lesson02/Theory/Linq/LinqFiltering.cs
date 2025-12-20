using System.Linq;
using Models.Employees;

namespace Playground.Lesson02;

public static class LinqFiltering
{
    public static void RunExamples(IEnumerable<Employee> employees)
    {
        System.Console.WriteLine("\nLinq Filtering Examples:");
        System.Console.WriteLine("--------------------------------");

        // Existing example: veterinarians
        var vets = employees.Where(e => e.Role == WorkRole.Veterinarian);
        vets.ToList().ForEach(v =>
            System.Console.WriteLine($"Vet: {v.FirstName} {v.LastName}, Hired: {v.HireDate:d}, Cards: {v.CreditCards.Count}")
        );

        System.Console.WriteLine();

        // Take: first 5 employees
        var first5 = employees.Take(5);
        System.Console.WriteLine("First 5 employees:");
        first5.ToList().ForEach(e => System.Console.WriteLine($"{e.FirstName} {e.LastName}"));

        System.Console.WriteLine();

        // TakeWhile: order by HireDate then take while hired before 2010
        var orderedByHire = employees.OrderBy(e => e.HireDate);
        var hiredBefore2010 = orderedByHire.TakeWhile(e => e.HireDate.Year < 2010);
        System.Console.WriteLine("Employees hired before 2010 (TakeWhile on ordered list):");
        hiredBefore2010.ToList().ForEach(e => System.Console.WriteLine($"{e.FirstName} {e.LastName} - {e.HireDate:d}"));

        System.Console.WriteLine();

        // Skip: skip first 10 employees and show next 5
        var afterFirst10 = employees.Skip(10);
        System.Console.WriteLine("Employees after skipping first 10 (showing 5):");
        afterFirst10.Take(5).ToList().ForEach(e => System.Console.WriteLine($"{e.FirstName} {e.LastName}"));

        System.Console.WriteLine();

        // SkipWhile: skip until the first Veterinarian, then show next 5
        var skipUntilVet = employees.SkipWhile(e => e.Role != WorkRole.Veterinarian);
        System.Console.WriteLine("After SkipWhile until first Veterinarian (showing 5):");
        skipUntilVet.Take(5).ToList().ForEach(e => System.Console.WriteLine($"{e.FirstName} {e.LastName} - {e.Role}"));
    }
}
