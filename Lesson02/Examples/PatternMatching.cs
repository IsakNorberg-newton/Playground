using System.Globalization;

namespace Playground.Lesson02;

public static class PatternMatching
{
    public record Shape (double Width, double Height);

    //Type pattern combined with Property pattern
    static string VariousPatternMatching(object obj) => obj switch
    {
        //Constant Pattern
        true => "True",
        5 => "Five",
        5.3F => "Five comma three",

        //Relational Pattern and Pattern Combination
        < 10 and > 2 => "Less than 10 and larger than 2",

        //Type Pattern
        float => "type of float",

        //Type pattern with declaration pattern using when
        int i when i < 3 => "integer less than 3",

        //Type Pattern, Property Pattern and Relational Pattern
        string { Length: > 4 } => "string with length > 4",

        //Type pattern with declaration pattern using when
        Shape { Width: 50 } s when s.Height > 100 => "Shape with Width = 50 and Height > 100",

        //Type Pattern and Property Pattern
        Uri { Scheme: "https", Port: 443 } => "Uri with Scheme=https and Port=443",

        //Type Pattern, Property Pattern, Relatinal Pattern, Var Pattern
        Uri { Scheme: "http", Port: < 80, Host: var host } when host.Length < 1000 => "host.Lenght < 1000",

        //Discard Pattern
        _ => "discarded"
    };

    //Classic Relational Pattern and Constant Pattern
    public static string GetWeightCategory(decimal bmi) => bmi switch
    {
        < 18.5m => "underweight",
        < 25m => "normal",
        < 30m => "overweight",
        35 => "BMI exactly 50",
        _ => "obese"
    };

    //Var Pattern    
    public static bool IsJanetOrJohn(string name) =>
        name.ToUpper() is var upper && (upper == "JANET" || upper == "JOHN");

    //Pattern Combinators
    public static bool IsVowel(char c) => c is 'a' or 'e' or 'i' or 'o' or 'u';

    public static bool Between1And9(int n) => n is >= 1 and <= 9;

    public static bool IsLetter(char c) => c is >= 'a' and <= 'z'
                                or >= 'A' and <= 'Z';

    // ============================================================
    // Tuple Pattern Matching Examples
    // ============================================================
    
    // Rock, Paper, Scissors game using tuple pattern matching
    public static string RockPaperScissors(string player1, string player2) =>
        (player1, player2) switch
        {
            ("rock", "rock") => "Tie",
            ("rock", "paper") => "Player 2 wins - Paper covers Rock",
            ("rock", "scissors") => "Player 1 wins - Rock crushes Scissors",
            ("paper", "rock") => "Player 1 wins - Paper covers Rock",
            ("paper", "paper") => "Tie",
            ("paper", "scissors") => "Player 2 wins - Scissors cut Paper",
            ("scissors", "rock") => "Player 2 wins - Rock crushes Scissors",
            ("scissors", "paper") => "Player 1 wins - Scissors cut Paper",
            ("scissors", "scissors") => "Tie",
            _ => "Invalid input"
        };


    // Traffic light state machine using tuple pattern matching
    public static string NextTrafficLight(string currentLight, bool pedestrianWaiting) =>
        (currentLight, pedestrianWaiting) switch
        {
            ("green", true) => "yellow",
            ("green", false) => "green",
            ("yellow", _) => "red",
            ("red", _) => "green",
            _ => "error"
        };


    public static void RunExamples()
    {
        Console.WriteLine("\n===Pattern Matching Examples ===\n");
        //Relational Pattern with object type
        object obj = 2m;                  // decimal
        Console.WriteLine(obj is < 3m);  // True
        Console.WriteLine(obj is < 3);   // False

        //Not pattern
        Console.WriteLine(obj is not string);
        Console.WriteLine(obj is decimal);
        Console.WriteLine();

        //Various Pattern Matching
        //Test Constant and Relational Pattern
        Console.WriteLine(VariousPatternMatching(true));   // True
        Console.WriteLine(VariousPatternMatching(5));      // Five
        Console.WriteLine(VariousPatternMatching(5.3F));   // Five comma three
        Console.WriteLine(VariousPatternMatching(7));      // Less than 10 and larger than 2

        //Test Type Pattern and Declaration Pattern
        Console.WriteLine(VariousPatternMatching(11.3F));  // Type of float
        Console.WriteLine(VariousPatternMatching(1));      // integer less than 3

        //Test Type Pattern, Property Pattern and Relational Pattern
        Console.WriteLine(VariousPatternMatching("The quick brown fox")); // string with length > 4

        //Test Type pattern with declaration pattern using when
        Console.WriteLine(VariousPatternMatching(
            new Shape(50, 200)));      // Shape with Width = 50 and Height > 100

        //Test Type Pattern and Property Pattern
        Console.WriteLine(VariousPatternMatching(
            new Uri("https://localhost:443")));            // Uri with Scheme = https and Port = 443

        //Test Type Pattern, Property Pattern, Relatinal Pattern, Var Pattern
        Console.WriteLine(VariousPatternMatching(
            new Uri("http://localhost:60")));             // host.Lenght < 1000

        //Test Discard Pattern
        Console.WriteLine(VariousPatternMatching(100L));  // discarded


        //Other Pattern Matching Examples
        System.Console.WriteLine(GetWeightCategory(22.5m));  // normal
        System.Console.WriteLine(IsJanetOrJohn("janet"));    // True
        System.Console.WriteLine(IsVowel('e'));              // True
        System.Console.WriteLine(Between1And9(5));          // True
        System.Console.WriteLine(IsLetter('G'));            // True 

        Console.WriteLine("\n=== End of Pattern Matching Examples ===\n");

        Console.WriteLine("\n=== Tuple Pattern Matching Examples ===\n");

        // Example 1: Rock, Paper, Scissors
        Console.WriteLine("1. Rock, Paper, Scissors Game:");
        var games = new[]
        {
            ("rock", "scissors"),
            ("paper", "rock"),
            ("scissors", "paper"),
            ("rock", "rock")
        };

        foreach (var (p1, p2) in games)
        {
            Console.WriteLine($"   {p1} vs {p2}: {RockPaperScissors(p1, p2)}");
        }
        Console.WriteLine();

        // Example 3: Traffic light state machine
        Console.WriteLine("3. Traffic Light State Machine:");
        var lightStates = new[]
        {
            ("green", false),
            ("green", true),
            ("yellow", false),
            ("red", true)
        };

        foreach (var (light, pedestrian) in lightStates)
        {
            var next = NextTrafficLight(light, pedestrian);
            var pedStatus = pedestrian ? "waiting" : "not waiting";
            Console.WriteLine($"   {light} (pedestrian {pedStatus}) -> {next}");
        }

        Console.WriteLine("\n=== End of Tuple Pattern Matching Examples ===\n");

    }
}
