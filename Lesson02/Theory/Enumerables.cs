namespace Playground.Lesson02;

public static class Enumerables
{
    public static void RunExamples()
    {
        Console.WriteLine("\n=== Enumerables Examples ===\n");

        //All Strings, Arrays and Collections in .NET are enumerable
        var array = new int[] { 1, 2, 3, 4, 5 };
        foreach (var item in array)
        {
            Console.WriteLine(item); // 1, 2, 3, 4, 5
        }

        Console.WriteLine();

        var list = new List<int> { 1, 2, 3 };
        foreach (var item in list)
        {
            Console.WriteLine(item); // 1, 2, 3
        }

        Console.WriteLine();

        //enumerate a string
        foreach (char c in "beer")
        {
            Console.WriteLine(c); // b, e, e, r,
        }

        Console.WriteLine();

        //Same as above mor more explicit showing the enumerator usage
        using (var enumerator = "beer".GetEnumerator())
            while (enumerator.MoveNext())
            {
                var element = enumerator.Current;
                Console.WriteLine(element); // b, e, e, r,
            }

        Console.WriteLine("\n=== End of Enumerables Examples ===\n");
    }
}
