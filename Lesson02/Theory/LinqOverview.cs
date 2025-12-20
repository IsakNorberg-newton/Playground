
using System.Linq;
using Models.Employees;
using Seido.Utilities.SeedGenerator;

namespace Playground.Lesson02;

public static class LinqOverview
{
    public static void RunExamples()
    {
        Console.WriteLine("\n=== Linq Examples ===\n");

        var seeder = new SeedGenerator();
        var employees = seeder.ItemsToList<Employee>(100);

        // Demonstrating filtering examples (Where, Take, TakeWWhile, Skip, SkipWhile)
        LinqFiltering.RunExamples(employees);

        // Demonstrating projecting examples (Select, SelectMany)
        LinqProjecting.RunExamples(employees);

        // Demonstrating joining examples (Join, GroupJoin, Zip)
        LinqJoining.RunExamples(employees);

        // Demonstrating ordering examples (OrderBy, ThenBy, OrderByDescending, ThenByDescending, Reverse)
        LinqOrdering.RunExamples(employees);

        // Demonstrating grouping examples (GroupBy, ToLookup)
        LinqGrouping.RunExamples(employees);
        
        // Demonstrating set operations examples (Distinct, Union, Intersect, Except)
        LinqSetOps.RunExamples(employees);

        // Demonstrating conversion Import examples (OfType, Cast)
        LinqConversionImport.RunExamples(employees);

        // Demonstrating conversion Export examples (ToList, ToArray, ToDictionary, AsEnumerable, AsQueryable)
        LinqConversionExport.RunExamples(employees);

        // Demonstrating element operations examples (First, FirstOrDefault, Last, LastOrDefault, Single, SingleOrDefault, ElementAt, ElementAtOrDefault, DefaultIfEmpty)
        LinqElementOps.RunExamples(employees);

        // Demonstrating aggregation examples (Count, LongCount, Sum, Min, Max, Average, Aggregate)
        LinqAggregation.RunExamples(employees);

        // Demonstrating quantifier examples (Any, All, Contains)
        LinqQuantifier.RunExamples(employees);

        // Demonstrating generation examples (Empty, Range, Repeat)
        LinqGeneration.RunExamples(employees);

        Console.WriteLine("\n=== End of Linq Examples ===\n");
    }
}
