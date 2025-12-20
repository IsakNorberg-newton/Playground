namespace Playground.Lesson02;

// Simple extension methods for demonstration
public static class StringExtensions
{
    // Extends string with a method that capitalizes the first letter
    public static string Capitalize(this string value)
        => string.IsNullOrEmpty(value)
            ? value
            : char.ToUpper(value[0]) + value[1..];

    // Extends string with a method that repeats it n times
    public static string Repeat(this string value, int count)
    {
        var result = "";
        for (int i = 0; i < count; i++)
            result += value;
        return result;
    }
}

// Extension method for IEnumerable<T> - just like LINQ methods
public static class EnumerableExtensions
{
    // Counts elements that satisfy a predicate
    public static int CountWhere<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        int count = 0;
        foreach (var item in source)
        {
            if (predicate(item))
                count++;
        }
        return count;
    }

    public static IEnumerable<T> TakeEveryNth<T>(this IEnumerable<T> source, int n)
    {
        int index = 0;
        foreach (var item in source)
        {
            if (index % n == 0)
                yield return item;
            index++;
        }
    }
}


// Give a Type superpower to wrap itself into a single-item list
public static class GenericExtensions
{
    // Extension method that wraps any value T into a single-element list
    public static List<T> ToSingleItemList<T>(this T value)
    {
        return new List<T> { value };
    }
}

public static class Extensions
{
    public static void RunExamples()
    {
        Console.WriteLine("\n=== Extensions Examples ===\n");

        // Simple extension methods for demonstration
        string name = "martin";

        // Using extension methods (and chaining them)
        var result = name
            .Capitalize()
            .Repeat(3);

        System.Console.WriteLine(result);

        var numbers = new List<int> { 1, 5, 10, 15, 20 };


        // Extension method for IEnumerable<T> - just like LINQ methods
       int count = numbers.OrderDescending().CountWhere(n => n > 10);
        Console.WriteLine($"Numbers > 10: {count}");

        // Use it like any LINQ method
        var every3rd = numbers.TakeEveryNth(3);
        Console.WriteLine("Every 3rd number: " + string.Join(", ", every3rd));

        // Give a Type superpower to wrap itself into a single-item list
        int number = 42;
        string text = "hello";
        DateTime now = DateTime.Now;

        var list1 = number.ToSingleItemList();   // List<int> { 42 }
        var list2 = text.ToSingleItemList();     // List<string> { "hello" }
        var list3 = now.ToSingleItemList();      // List<DateTime> { <timestamp> }

        Console.WriteLine(string.Join(", ", list1));
        Console.WriteLine(string.Join(", ", list2));
        Console.WriteLine(string.Join(", ", list3));

        Console.WriteLine("\n=== End of Extensions Examples ===\n");
    }
}

