using Microsoft.AspNetCore.Identity;
using Models.Employees;

namespace Playground.Lesson02;

public static class Generics
{
    // Simple generic class
    public class Box<T>
    {
        private T _content;

        public Box(T content)
        {
            _content = content;
        }

        public T GetContent() => _content;
        public void SetContent(T content) => _content = content;

        public override string ToString() => $"Box contains: {_content}";
    }

    // Generic class with multiple type parameters
    public class Pair<TFirst, TSecond>
    {
        public TFirst First { get; set; }
        public TSecond Second { get; set; }

        public Pair(TFirst first, TSecond second)
        {
            First = first;
            Second = second;
        }

        public override string ToString() => $"({First}, {Second})";
    }

    // Generic class with constraints
    public class Repository<T> where T : class, new()
    {
        private List<T> _items = new();

        public void Add(T item) => _items.Add(item);
        public IEnumerable<T> GetAll() => _items;
        public T CreateNew() => new T();
        public int Count => _items.Count;
    }

    public static void RunExamples()
    {
        Console.WriteLine("\n=== Generics Examples ===\n");

        Console.WriteLine("Generic Classes:\n");

        var intBox = new Box<int>(42);
        var stringBox = new Box<string>("Hello Generics");
        var dateBox = new Box<DateTime>(DateTime.Now);

        Console.WriteLine($"   {intBox}");
        Console.WriteLine($"   {stringBox}");
        Console.WriteLine($"   {dateBox}");

        // Multiple type parameters
        var pair1 = new Pair<string, int>("Age", 25);
        var pair2 = new Pair<int, double>(1, 3.14);
        Console.WriteLine($"   {pair1}");
        Console.WriteLine($"   {pair2}");
        Console.WriteLine();


        Console.WriteLine("Repository Pattern with Constraints:\n");

        var personRepo = new Repository<Employee>();
        personRepo.Add(new Employee (default, "Bob", default, default, WorkRole.Maintenance, default));
        personRepo.Add(new Employee (default, "Alice", default, default, WorkRole.ProgramCoordinator, default));
        
        Console.WriteLine($"   Repository count: {personRepo.Count}");
        Console.WriteLine("   All items:");
        foreach (var person in personRepo.GetAll())
        {
            Console.WriteLine($"   - {person}");
        }

        Console.WriteLine("Some Built-in Generic Collections:\n");

        var list = new List<int> { 1, 2, 3, 4, 5 };
        var dict = new Dictionary<string, int>
        {
            ["one"] = 1,
            ["two"] = 2,
            ["three"] = 3
        };
        var set = new HashSet<string> { "apple", "banana", "cherry", "apple" }; // duplicate ignored

        Console.WriteLine($"   List<int>: {string.Join(", ", list)}");
        Console.WriteLine($"   Dictionary<string, int>: {string.Join(", ", dict.Select(kv => $"{kv.Key}={kv.Value}"))}");
        Console.WriteLine($"   HashSet<string>: {string.Join(", ", set)}");
        Console.WriteLine();

        Console.WriteLine("\n=== End of Generics Examples ===\n");
    }
}
