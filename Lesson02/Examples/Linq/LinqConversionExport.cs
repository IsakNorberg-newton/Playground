using System.Linq;
using Models.Employees;

namespace Playground.Lesson02;

public static class LinqConversionExport
{
    public static void RunExamples(IEnumerable<Employee> employees)
    {
        System.Console.WriteLine("\nLinq Conversion Export Examples:");
        System.Console.WriteLine("--------------------------------");
        // ToList / ToArray: materialize sequences
        var empList = employees.Take(10).ToList();
        var empArray = employees.Skip(10).Take(5).ToArray();
        System.Console.WriteLine($"ToList count: {empList.Count}, ToArray length: {empArray.Length}");

        System.Console.WriteLine();

        // ToDictionary: create a dictionary keyed by CreditCard issuer -> list of employees who have such a card
        var dict = employees
            .SelectMany(e => e.CreditCards.Select(cc => new { cc.Issuer, Employee = e }))
            .GroupBy(x => x.Issuer)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.Employee).Where(emp => emp.CreditCards.Any(cc => cc.Issuer == g.Key))
                      .ToList()
            );

        if (dict.Any())
        {
            var sampleIssuer = dict.Keys.First();
            System.Console.WriteLine($"ToDictionary by issuer example: found issuer {sampleIssuer} with {dict[sampleIssuer].Count} employees");
        }

        System.Console.WriteLine();

        // AsEnumerable / AsQueryable: change compile-time type without forcing execution
        var asEnum = employees.Where(e => e.HireDate.Year > 2010).AsEnumerable();
        var asQuery = employees.AsQueryable().Where(e => e.Role == WorkRole.Veterinarian).AsQueryable();
        System.Console.WriteLine($"AsEnumerable deferred sequence type: {asEnum.GetType().Name}");
        System.Console.WriteLine($"AsQueryable sequence type: {asQuery.GetType().Name}");
    
        /*
         * IEnumerable vs IQueryable
         * - Purpose: IEnumerable<T> is for in-memory (LINQ-to-Objects). IQueryable<T> is for provider-backed
         *   queries (EF, OData) that translate expression trees to provider-native queries (SQL, etc.).
         * - Execution: IEnumerable executes using delegates on the client; IQueryable builds Expression trees
         *   which the provider translates and executes (deferred execution at the provider).
         * - AsEnumerable vs AsQueryable: AsEnumerable() casts to IEnumerable<T> and forces subsequent
         *   operations to run in-memory; AsQueryable() exposes an IQueryable<T> so a provider may translate
         *   the query (wrapping an in-memory sequence with AsQueryable() does not create a remote provider).
         * - When to use: prefer IQueryable to push work to the data source (avoid fetching unnecessary rows).
         *   Use IEnumerable/AsEnumerable when you need client-side operations or the provider cannot translate
         *   the expression. Be mindful of performance: non-translatable operations may execute client-side
         *   after fetching data, which can be costly.
         */
    
    
    }
}
