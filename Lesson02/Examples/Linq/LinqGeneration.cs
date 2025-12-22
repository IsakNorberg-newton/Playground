using System.Linq;
using Models.Employees;
using Seido.Utilities.SeedGenerator;

namespace Playground.Lesson02;

public static class LinqGeneration
{
    public static void RunExamples(IEnumerable<Employee> employees)
    {
        System.Console.WriteLine("\nLinq Generation Examples:");
        System.Console.WriteLine("--------------------------------");

        // Materialize employees for repeated/lookup operations
        var list = employees.ToList();

        // Empty: create an empty sequence of Employee
        var empty = Enumerable.Empty<Employee>();
        System.Console.WriteLine($"Empty<Employee> count: {empty.Count()}, any: {empty.Any()}");

        // Range: create a list of CreditCard instances using Enumerable.Range
        var seeder = new SeedGenerator();
        var cards = Enumerable.Range(1, 5)
            .Select(i => new CreditCard().Seed(seeder))
            .ToList();
        System.Console.WriteLine($"Range(1,5) created credit cards: {cards.Count}");
        foreach (var c in cards)
            System.Console.WriteLine($" - {c.Issuer} {c.Number} exp {c.ExpirationMonth}/{c.ExpirationYear} Id={c.CreditCardId}");

        // Repeat: repeat a sample employee multiple times
        var sample = list.FirstOrDefault();
        if (sample is not null)
        {
            var repeated = Enumerable.Repeat(sample, 3).ToList();
            System.Console.WriteLine($"Repeat sample 3 times: count={repeated.Count}, all equal by value: {repeated.All(r => r == sample)}");
        }
        else
        {
            System.Console.WriteLine("No employees available to demonstrate Repeat.");
        }

        // Compare LINQ generation to imperative loops
        // Range vs imperative for creating a list of ints
        var rangeLinq = Enumerable.Range(1, 5).ToList();
        var rangeImperative = new List<int>();
        for (int i = 1; i <= 5; i++)
        {
            rangeImperative.Add(i);
        }
        System.Console.WriteLine($"Range equals imperative: {rangeLinq.SequenceEqual(rangeImperative)}");

        // Repeat vs imperative: create repeated employee references
        if (sample is not null)
        {
            var repeatLinq = Enumerable.Repeat(sample, 3).ToList();
            var repeatImperative = new List<Employee>();
            for (int i = 0; i < 3; i++)
            {
                repeatImperative.Add(sample);
            }
            System.Console.WriteLine($"Repeat equals imperative (SequenceEqual): {repeatLinq.SequenceEqual(repeatImperative)}");
        }
    }
}
