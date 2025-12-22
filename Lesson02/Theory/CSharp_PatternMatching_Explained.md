# C# Pattern Matching: A Functional Programming Perspective

## Introduction

Pattern matching is one of the most powerful features in modern C#, enabling declarative, expressive code that closely aligns with functional programming principles. Introduced in C# 7.0 and significantly enhanced in C# 8.0, 9.0, and beyond, pattern matching allows you to test and deconstruct values in a concise, readable way.

## Why Pattern Matching Matters in Functional Programming

Functional programming emphasizes:
- **Declarative code**: Express *what* you want, not *how* to get it
- **Expression-oriented programming**: Everything returns a value
- **Pattern-based dispatch**: Route logic based on data structure and values
- **Exhaustive matching**: Handle all possible cases explicitly

Pattern matching provides:
1. Concise conditional logic based on types, values, and structure
2. Type-safe value extraction and deconstruction
3. Switch expressions that return values (functional style)
4. Elimination of complex if-else chains
5. Compile-time exhaustiveness checking (with some patterns)

## Core Pattern Types

### 1. Constant Pattern

Match against specific constant values:

```csharp
static string VariousPatternMatching(object obj) => obj switch
{
    true => "True",
    5 => "Five",
    5.3F => "Five comma three",
    // ...
};
```

**Usage:**
```csharp
Console.WriteLine(VariousPatternMatching(true));   // True
Console.WriteLine(VariousPatternMatching(5));      // Five
Console.WriteLine(VariousPatternMatching(5.3F));   // Five comma three
```

**Characteristics:**
- Match exact values
- Type must match exactly (5 vs 5.3F are different)
- Perfect for state machines and specific value handling

### 2. Relational Pattern

Use comparison operators to match ranges:

```csharp
public static string GetWeightCategory(decimal bmi) => bmi switch
{
    < 18.5m => "underweight",
    < 25m => "normal",
    < 30m => "overweight",
    35 => "BMI exactly 35",
    _ => "obese"
};
```

**Supported operators:** `<`, `<=`, `>`, `>=`

**Usage:**
```csharp
Console.WriteLine(GetWeightCategory(22.5m));  // normal
```

**Why It Matters:**
- Eliminates verbose if-else chains
- More declarative and readable
- Evaluated in order (first match wins)
- Common in functional languages like F# and Scala

**Important Note:** Patterns are evaluated **top to bottom**, so order matters:
```csharp
obj is < 3m   // True for decimal 2m
obj is < 3    // False for decimal 2m (type mismatch)
```

### 3. Type Pattern

Match based on the runtime type of an object:

```csharp
obj switch
{
    float => "type of float",
    string => "type of string",
    // ...
}
```

**Usage:**
```csharp
Console.WriteLine(VariousPatternMatching(11.3F));  // type of float
```

**Characteristics:**
- Tests if the value is of a specific type
- Often combined with other patterns
- Replaces traditional `is` type checks

### 4. Declaration Pattern (Type Pattern with Variable)

Combines type testing with variable declaration:

```csharp
obj switch
{
    int i when i < 3 => "integer less than 3",
    Shape { Width: 50 } s when s.Height > 100 => "Shape with Width = 50 and Height > 100",
    // ...
}
```

**Usage:**
```csharp
Console.WriteLine(VariousPatternMatching(1));  // integer less than 3
Console.WriteLine(VariousPatternMatching(new Shape(50, 200)));  
    // Shape with Width = 50 and Height > 100
```

**Key Points:**
- Declares a variable (`i`, `s`) that can be used in the pattern
- `when` clause adds additional conditions (guard clause)
- Variable is strongly typed and available in the result expression

### 5. Property Pattern

Match based on properties of an object:

```csharp
obj switch
{
    string { Length: > 4 } => "string with length > 4",
    Uri { Scheme: "https", Port: 443 } => "Uri with Scheme=https and Port=443",
    // ...
}
```

**Usage:**
```csharp
Console.WriteLine(VariousPatternMatching("The quick brown fox"));  
    // string with length > 4
Console.WriteLine(VariousPatternMatching(new Uri("https://localhost:443")));  
    // Uri with Scheme=https and Port=443
```

**Nested Property Patterns:**
```csharp
Uri { Scheme: "http", Port: < 80, Host: var host } when host.Length < 1000
```

This pattern:
1. Checks type is `Uri`
2. Checks `Scheme` equals "http"
3. Checks `Port` is less than 80 (relational pattern)
4. Captures `Host` in variable `host` (var pattern)
5. Adds guard clause checking `host.Length`

**Benefits:**
- Destructures objects inline
- No need for intermediate variables
- Can nest arbitrarily deep
- Type-safe property access

### 6. Var Pattern

Capture a value or subvalue in a variable:

```csharp
Uri { Scheme: "http", Port: < 80, Host: var host } when host.Length < 1000
```

**Standalone Usage, is expression:**
```csharp
public static bool IsJanetOrJohn(string name) =>
    name.ToUpper() is var upper && (upper == "JANET" || upper == "JOHN");
```

**Characteristics:**
- `is` expression
- Always matches (never fails)
- Useful for capturing intermediate values
- Enables functional-style transformations inline

### 7. Discard Pattern (`_`)

Match anything without capturing:

```csharp
obj switch
{
    // ... other cases ...
    _ => "discarded"
};
```

**Usage:**
```csharp
Console.WriteLine(VariousPatternMatching(100L));  // discarded
```

**Common Uses:**
- Default case in switch expressions
- Ignore parts of tuples: `(_, _, value)`
- Placeholder for "don't care" values

**Functional Significance:**
The discard pattern ensures **exhaustiveness** - all cases are handled, preventing null reference exceptions and making code more robust.

## Pattern Combinators

C# 9.0 introduced pattern combinators to compose complex patterns.

### `and` Combinator

Match when both patterns match:

```csharp
obj switch
{
    < 10 and > 2 => "Less than 10 and larger than 2",
    // ...
}

public static bool Between1And9(int n) => n is >= 1 and <= 9;

public static bool IsLetter(char c) => c is >= 'a' and <= 'z'
                                            or >= 'A' and <= 'Z';
```

**Usage:**
```csharp
Console.WriteLine(VariousPatternMatching(7));  // Less than 10 and larger than 2
Console.WriteLine(Between1And9(5));            // True
Console.WriteLine(IsLetter('G'));              // True
```

### `or` Combinator

Match when either pattern matches:

```csharp
public static bool IsVowel(char c) => c is 'a' or 'e' or 'i' or 'o' or 'u';
```

**Usage:**
```csharp
Console.WriteLine(IsVowel('e'));  // True
Console.WriteLine(IsVowel('x'));  // False
```

### `not` Combinator

Match when the pattern does NOT match:

```csharp
object obj = 2m;
Console.WriteLine(obj is not string);  // True
Console.WriteLine(obj is decimal);     // True
```

**Benefits:**
- More readable than negation operators
- Composable with other patterns
- Eliminates nested conditions

### Combining Combinators

```csharp
value is not null and >= 0 and < 100
```

## Tuple Pattern Matching

One of the most powerful functional features: match on multiple values simultaneously.

### Example 1: Rock, Paper, Scissors Game

```csharp
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
```

**Usage:**
```csharp
Console.WriteLine(RockPaperScissors("rock", "scissors"));  
    // Player 1 wins - Rock crushes Scissors
Console.WriteLine(RockPaperScissors("paper", "rock"));     
    // Player 1 wins - Paper covers Rock
```

**Why This Is Functional:**
- Declarative: states all possible combinations explicitly
- No mutable state
- Pure function: same inputs always produce same output
- Exhaustive: all cases covered (with `_` fallback)

### Example 2: State Machine (Traffic Light)

```csharp
public static string NextTrafficLight(string currentLight, bool pedestrianWaiting) =>
    (currentLight, pedestrianWaiting) switch
    {
        ("green", true) => "yellow",
        ("green", false) => "green",
        ("yellow", _) => "red",
        ("red", _) => "green",
        _ => "error"
    };
```

**Usage:**
```csharp
Console.WriteLine(NextTrafficLight("green", true));   // yellow
Console.WriteLine(NextTrafficLight("yellow", false)); // red
```

**Patterns Used:**
- Tuple matching on two values
- Constant pattern for strings and booleans
- Discard pattern (`_`) when second value doesn't matter

**Functional Benefits:**
- State transitions are explicit and declarative
- No mutable state variables
- Easy to reason about
- Compile-time verification of state coverage

## `is` Expression vs. `switch` Expression

### `is` Expression

Used for single pattern matching, often in conditionals:

```csharp
if (obj is string { Length: > 4 })
{
    // Handle long string
}

if (value is >= 1 and <= 9)
{
    // Handle value in range
}

// Inline use
var result = name.ToUpper() is var upper && (upper == "JANET" || upper == "JOHN");
```

### `switch` Expression

Used for multiple cases, returns a value:

```csharp
var result = obj switch
{
    null => "null value",
    string s => $"string: {s}",
    int i when i > 0 => "positive int",
    _ => "something else"
};
```

**Key Differences:**

| Feature | `is` Expression | `switch` Expression |
|---------|----------------|---------------------|
| Use Case | Single pattern test | Multiple patterns |
| Returns Value | Boolean or captures | Arbitrary value |
| Exhaustiveness | Not required | Recommended (use `_`) |
| Syntax | `obj is pattern` | `obj switch { ... }` |

## Functional Programming Patterns

### 1. Replacing Complex If-Else Chains

**❌ Imperative Style:**
```csharp
string GetCategory(decimal bmi)
{
    if (bmi < 18.5m)
        return "underweight";
    else if (bmi < 25m)
        return "normal";
    else if (bmi < 30m)
        return "overweight";
    else
        return "obese";
}
```

**✅ Functional Style with Pattern Matching:**
```csharp
string GetCategory(decimal bmi) => bmi switch
{
    < 18.5m => "underweight",
    < 25m => "normal",
    < 30m => "overweight",
    _ => "obese"
};
```

### 2. Type-Safe Dispatch

Handle different types without casting:

```csharp
string Process(object input) => input switch
{
    int i => $"Integer: {i * 2}",
    string s => $"String length: {s.Length}",
    double d => $"Double: {d:F2}",
    _ => "Unknown type"
};
```

### 3. Discriminated Unions (via Inheritance)

```csharp
abstract record Result;
record Success(string Value) : Result;
record Error(string Message) : Result;

string HandleResult(Result result) => result switch
{
    Success { Value: var v } => $"Success: {v}",
    Error { Message: var msg } => $"Error: {msg}",
    _ => "Unknown"
};
```

This is a functional programming staple from languages like F# and Haskell.

### 4. Validating and Transforming

```csharp
string ProcessInput(string input) => input switch
{
    null => "Null input",
    "" => "Empty input",
    { Length: < 3 } => "Too short",
    { Length: > 100 } => "Too long",
    var s when s.All(char.IsDigit) => $"Number: {s}",
    var s => $"Valid: {s}"
};
```

### 5. Multi-Value Decision Making

```csharp
string GetShippingCost(decimal weight, string country) =>
    (weight, country) switch
    {
        ( < 1, "US") => "$5",
        ( < 1, _) => "$10",
        ( < 5, "US") => "$10",
        ( < 5, _) => "$20",
        (_, "US") => "$20",
        _ => "$40"
    };
```

## Advanced Patterns

### Nested Property Patterns

```csharp
record Address(string City, string Country);
record Person(string Name, Address Address);

string CheckLocation(Person person) => person switch
{
    { Address: { Country: "US", City: "New York" } } => "New Yorker",
    { Address: { Country: "US" } } => "American",
    { Address: { Country: "Canada" } } => "Canadian",
    _ => "Other"
};
```

### Positional Patterns (with Deconstruction)

```csharp
public record Point(int X, int Y);

string Classify(Point point) => point switch
{
    (0, 0) => "Origin",
    (var x, 0) => $"On X-axis at {x}",
    (0, var y) => $"On Y-axis at {y}",
    (var x, var y) when x == y => "On diagonal",
    (var x, var y) => $"At ({x}, {y})"
};
```

### List Patterns (C# 11+)

```csharp
int[] numbers = { 1, 2, 3 };

string Describe(int[] arr) => arr switch
{
    [] => "Empty",
    [var x] => $"Single: {x}",
    [var x, var y] => $"Pair: {x}, {y}",
    [var first, .., var last] => $"First: {first}, Last: {last}",
    _ => "Many"
};
```

## Best Practices

### 1. Use Switch Expressions for Multiple Cases

```csharp
// ✅ Clean and functional
var result = value switch
{
    0 => "zero",
    1 => "one",
    _ => "many"
};

// ❌ Verbose and imperative
string result;
if (value == 0)
    result = "zero";
else if (value == 1)
    result = "one";
else
    result = "many";
```

### 2. Always Include a Default Case

```csharp
// ✅ Exhaustive
var result = obj switch
{
    string s => s,
    int i => i.ToString(),
    _ => "unknown"  // Prevents runtime errors
};
```

### 3. Order Patterns from Specific to General

```csharp
// ✅ Correct order
value switch
{
    5 => "five",           // Most specific
    < 10 => "less than 10",
    _ => "other"           // Most general
};

// ❌ Wrong order (unreachable code)
value switch
{
    < 10 => "less than 10",
    5 => "five",           // Never reached!
    _ => "other"
};
```

### 4. Use Property Patterns for Clarity

```csharp
// ✅ Clear and declarative
person switch
{
    { Age: >= 18, HasLicense: true } => "Can drive",
    _ => "Cannot drive"
};

// ❌ Less clear
person switch
{
    var p when p.Age >= 18 && p.HasLicense => "Can drive",
    _ => "Cannot drive"
};
```

### 5. Leverage Tuple Patterns for Complex Logic

```csharp
// ✅ Explicit and maintainable
(temperature, humidity) switch
{
    ( > 30, > 70) => "Hot and humid",
    ( > 30, _) => "Hot",
    ( < 10, _) => "Cold",
    _ => "Moderate"
};
```

## Pattern Matching vs. Traditional Approaches

### Polymorphism vs. Pattern Matching

**Polymorphism (OOP):**
```csharp
abstract class Shape
{
    public abstract double Area();
}

class Circle : Shape
{
    public double Radius { get; }
    public override double Area() => Math.PI * Radius * Radius;
}
```

**Pattern Matching (FP):**
```csharp
record Shape;
record Circle(double Radius) : Shape;
record Rectangle(double Width, double Height) : Shape;

double GetArea(Shape shape) => shape switch
{
    Circle { Radius: var r } => Math.PI * r * r,
    Rectangle { Width: var w, Height: var h } => w * h,
    _ => 0
};
```

**Trade-offs:**
- **Polymorphism**: Add new types easily, harder to add new operations
- **Pattern Matching**: Add new operations easily, harder to add new types
- **Pattern Matching**: All logic in one place (easier to see all cases)
- **Polymorphism**: Logic distributed across types

This is the classic **Expression Problem** in programming language design.

## Performance Considerations

- Pattern matching is highly optimized by the compiler
- Switch expressions often compile to jump tables (very fast)
- Property patterns may involve runtime checks
- No performance penalty compared to manual if-else chains
- Can be more efficient than polymorphic dispatch in some cases

## Conclusion

Pattern matching transforms C# into a more functional, expressive language by providing:

1. **Declarative Syntax**: Express intent clearly
2. **Type Safety**: Catch errors at compile time
3. **Exhaustiveness**: Handle all cases explicitly
4. **Composability**: Combine patterns with `and`, `or`, `not`
5. **Conciseness**: Reduce boilerplate code dramatically

In the context of functional programming, pattern matching enables:
- **Expression-oriented code**: Everything returns a value
- **Immutable data handling**: Transform rather than mutate
- **Discriminated unions**: Model domain with sum types
- **Pure functions**: Predictable, testable code
- **Declarative control flow**: What, not how

The examples in [PatternMatchingExamples.cs](Lesson02/PatternMatchingExamples.cs) demonstrate how pattern matching brings functional programming idioms to C#, making code more maintainable, safer, and more elegant. From simple value matching to complex tuple patterns and state machines, pattern matching is an essential tool in modern C# development.

Whether you're building decision trees, state machines, parsers, or domain models, pattern matching provides a powerful, functional approach that reduces bugs and improves code clarity.
