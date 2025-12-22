using System.Linq;
using Models.Employees;

namespace Playground.Lesson02;

public static class LinqConversionImport
{

    public static void RunExamples(IEnumerable<Employee> employees)
    {
        System.Console.WriteLine("\nLinq Conversion Import Examples:");
        System.Console.WriteLine("--------------------------------");

        var seeder = new Seido.Utilities.SeedGenerator.SeedGenerator();
        var temps = seeder.ItemsToList<TempEmployee>(100);

        // Create a mixed Employee sequence using seeded TempEmployee instances (no nulls)
        // Pick items from employees and temps: take 30 employees and 20 temps, then interleave them
        var empSlice = employees.Take(30).ToList();
        var tempSlice = temps.Take(20).ToList();

        var max = Math.Max(empSlice.Count, tempSlice.Count);
        var mixedEmployees = new List<Employee>(empSlice.Count + tempSlice.Count);
        for (int i = 0; i < max; i++)
        {
            if (i < empSlice.Count) mixedEmployees.Add(empSlice[i]);
            if (i < tempSlice.Count) mixedEmployees.Add(tempSlice[i]);
        }

        // OfType<T>: filters by the derived type
        var onlyDerived = mixedEmployees.OfType<TempEmployee>();
        System.Console.WriteLine("OfType<TempEmployee> results (derived instances):");
        foreach (var e in onlyDerived)
            System.Console.WriteLine($"{e.FirstName} {e.LastName} - {e.Role} [{e.GetType().Name}]");

        System.Console.WriteLine();

        // Cast<T>: cast a sequence of derived instances back to Employee (upcast)
        var castBack = onlyDerived.Cast<Employee>();
        System.Console.WriteLine("Cast<TempEmployee> -> Cast<Employee> results:");
        foreach (var e in castBack){

            System.Console.WriteLine($"{e.FirstName} {e.LastName} - {e.Role} [{e.GetType().Name}]");
            //System.Console.WriteLine(e.FinalDate); //not accessible from Employee reference
        }
    }
}
