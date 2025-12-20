using System.Linq;
using Models.Employees;

namespace Playground.Lesson02;

public static class LinqAggregation
{
    public static void RunExamples(IEnumerable<Employee> employees)
    {
        System.Console.WriteLine("\nLinq Aggregation Examples:");
        System.Console.WriteLine("--------------------------------");


        // Aggregation examples
        // Aggregate: combine names into a single comma-separated string
        var names = employees.Take(5).Select(e => e.FirstName + " " + e.LastName).ToList();
        var aggregatedNames = names.Aggregate((a, b) => a + ", " + b);
        System.Console.WriteLine("Aggregate (first 5 names): " + aggregatedNames);

        // Aggregate with a starting value (seed): start at 0 and sum name lengths
        var totalNameCharsWithSeed = names.Aggregate(0, (acc, next) => acc + next.Length);
        System.Console.WriteLine($"Aggregate with seed (initial 0) total name chars: {totalNameCharsWithSeed}");

        System.Console.WriteLine();

        // Average, Sum, Min, Max on numeric projections (use HireDate.Year as an example numeric value)
        var years = employees.Select(e => e.HireDate.Year);
        var avgYear = years.Average();
        var sumYears = years.Sum();
        var minYear = years.Min();
        var maxYear = years.Max();
        System.Console.WriteLine($"Hire year: Avg={avgYear:F1}, Sum={sumYears}, Min={minYear}, Max={maxYear}");

        System.Console.WriteLine();

        // Count and LongCount
        var total = employees.Count();
        var vetsCount = employees.Count(e => e.Role == WorkRole.Veterinarian);
        var longTotal = employees.LongCount();
        System.Console.WriteLine($"Count: total={total}, vets={vetsCount}, LongCount={longTotal}");

        System.Console.WriteLine();

        // Sum example: total number of credit cards across employees
        var totalCards = employees.SelectMany(e => e.CreditCards).Count();
        System.Console.WriteLine($"Total credit cards (across employees): {totalCards}");


    }
}
