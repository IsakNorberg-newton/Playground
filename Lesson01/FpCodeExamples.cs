namespace Playground.Lesson01;

public static class FpCodeExamples
{
    public record Person(string Name, int Age);
    public record Circle(double Radius);
    public record Rectangle(double Width, double Height);

    public static void RunExamples()
    {
        //Immutability with Records
        var person1 = new Person("Alice", 30);

        // Records are immutable: you can't change Age directly.
        // Instead, you create a new instance with 'with' expression:
        var person2 = person1 with { Age = 31 };

        Console.WriteLine(person1); // Person { Name = Alice, Age = 30 }
        Console.WriteLine(person2); // Person { Name = Alice, Age = 31 }


        //Higher-Order Functions with Func
        Func<int, int, int> add = (x, y) => x + y;

        Func<Func<int, int, int>, int> applyTwice = f => f(2, 3) + f(4, 5);

        int result = applyTwice(add);
        Console.WriteLine(result); // (2+3) + (4+5) = 14

        
        //Expression-Based Programming with LINQ
        var numbers = new[] { 1, 2, 3, 4, 5 };
        var squares = numbers.Select(n => n * n);

        foreach (var s in squares)
            Console.WriteLine(s); // 1, 4, 9, 16, 25

    
        //Referential Transparency with Pure Functions
        int Square(int x) => x * x;

        Console.WriteLine(Square(5)); // Always 25
        Console.WriteLine(Square(5)); // Always 25, no side effects


        //Recursion
        int Factorial(int n)
        {
            if (n == 0) return 1;
            return n * Factorial(n - 1);
        }

        Console.WriteLine(Factorial(5)); // 120


        //Pattern Matching with Switch Expressions
        string DescribeShape(object shape) => shape switch
        {
            Circle c => $"Circle with radius {c.Radius}",
            Rectangle r => $"Rectangle {r.Width}x{r.Height}",
            _ => "Unknown shape"
        };

        Console.WriteLine(DescribeShape(new Circle(5)));
        Console.WriteLine(DescribeShape(new Rectangle(3, 4)));


        //Statelessness with Pure Functions
        // Pure function: no shared state, no side effects
        int AddNumbers(int a, int b) => a + b;

        var sum = AddNumbers(10, 20);
        Console.WriteLine(sum); // 30
    }
}
