namespace Playground.Lesson01;

public static class ImperativeVsDeclarative1
{
    public static void RunExamples()
    {
        //Imperative vs Declarative Example1

        //Imperative Style
        List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6 };
        List<int> evenSquares = new List<int>();

        foreach (int n in numbers)
        {
            if (n % 2 == 0) // imperative condition
            {
                int square = n * n;
                evenSquares.Add(square);
            }
        }

        foreach (int result in evenSquares)
        {
            Console.WriteLine(result);
        }

        //Declarative Style
        evenSquares = numbers
            .Where(n => n % 2 == 0) // declarative filtering
            .Select(n => n * n)     // declarative transformation
            .ToList();

        foreach (var result in evenSquares)
        {
            Console.WriteLine(result);
        }
    }
}
