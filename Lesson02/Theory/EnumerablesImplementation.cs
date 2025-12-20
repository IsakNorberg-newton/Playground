using System.Collections;
using System.Collections.Generic;

namespace Playground.Lesson02;
public static class EnumerablesImplementation
{
    //Simple enumerable method using yield return
    public static IEnumerable<string> MethodExample()
    {
        yield return "Good morning";
        yield return "Good afternoon";
        yield return "Good evening";
    }

    //Iterator using yield return (simplest)
    public class IteratorExample : IEnumerable<int>
    {
        public IEnumerator<int> GetEnumerator()
        {
            yield return 1;
            yield return 2;
            yield return 3;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    //Using an existing enumerable’s enumerator
    public class WrappedEnumerable : IEnumerable<int>
    {
        private readonly List<int> _data = new() { 1, 2, 3 };

        public IEnumerator<int> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    //Manual implementation of IEnumerable<T> and IEnumerator<T>
    public class ManualEnumerable : IEnumerable<int>, IEnumerator<int>
    {
        private int _index = -1;
        private readonly int[] _data = { 1, 2, 3 };

        public int Current => _data[_index];
        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            _index++;
            return _index < _data.Length;
        }

        public void Reset() => _index = -1;
        public void Dispose() { }

        public IEnumerator<int> GetEnumerator() => this;
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    public static void RunExamples()
    {
        Console.WriteLine("\n=== Enumerables Implementation Examples ===\n");

        foreach (var item in MethodExample())
            Console.WriteLine($"Method: {item}");

        //Enumerable Examples
        foreach (var x in new IteratorExample())
            Console.WriteLine($"Iterator: {x}");

        foreach (var x in new WrappedEnumerable())
            Console.WriteLine($"Wrapped: {x}");

        foreach (var x in new ManualEnumerable())
            Console.WriteLine($"Manual: {x}");

        Console.WriteLine("\n=== End of Enumerables Implementation Examples ===\n");
    }
}
