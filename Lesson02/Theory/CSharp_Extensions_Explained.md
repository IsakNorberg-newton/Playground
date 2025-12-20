# C# Extension Methods: A Functional Programming Perspective

## Introduction

Extension methods are a powerful C# feature that allows you to add methods to existing types without modifying their source code or creating derived types. This capability is fundamental to functional programming in C#, enabling fluent APIs, method chaining, and the creation of higher-order functions like those found in LINQ.

## Why Extension Methods Matter in Functional Programming

Extension methods enable:
1. **Fluent APIs**: Chain operations together naturally
2. **Non-invasive enhancement**: Add functionality without inheritance
3. **Pipeline-style programming**: Transform data through a series of operations
4. **Higher-order functions**: Create generic, reusable operations
5. **Composability**: Build complex operations from simple primitives

All of LINQ's functional-style methods (`Select`, `Where`, `SelectMany`, etc.) are extension methods.

## Syntax and Declaration

Extension methods are static methods in static classes, with the first parameter marked by the `this` keyword:

```csharp
public static class StringExtensions
{
    public static string Capitalize(this string value)
        => string.IsNullOrEmpty(value)
            ? value
            : char.ToUpper(value[0]) + value[1..];
}
```

**Key Components:**
- `static class` - Must be a static class
- `static method` - Extension method must be static
- `this string value` - First parameter with `this` keyword defines the extended type
- Can be called as if it's an instance method: `value.Capitalize()`

## String Extensions - Basic Examples

### Capitalize Method

```csharp
public static string Capitalize(this string value)
    => string.IsNullOrEmpty(value)
        ? value
        : char.ToUpper(value[0]) + value[1..];
```

**Usage:**
```csharp
string name = "martin";
var capitalized = name.Capitalize();  // "Martin"
```

**What it does:**
- Extends the `string` type
- Capitalizes the first letter
- Handles null/empty strings safely
- Returns a new string (immutable)

### Repeat Method

```csharp
public static string Repeat(this string value, int count)
{
    var result = "";
    for (int i = 0; i < count; i++)
        result += value;
    return result;
}
```

**Usage:**
```csharp
string text = "ha";
var repeated = text.Repeat(3);  // "hahaha"
```

**What it does:**
- Takes an additional parameter `count`
- Repeats the string `count` times
- Extension methods can have multiple parameters (first is always `this`)

### Method Chaining

The power of extension methods shines when chaining them:

```csharp
string name = "martin";

var result = name
    .Capitalize()
    .Repeat(3);

Console.WriteLine(result);  // "MartinMartinMartin"
```

**Why This Works:**
- Each method returns a value (string)
- Next method in chain operates on that value
- Creates a pipeline of transformations
- Reads left-to-right, top-to-bottom (natural flow)

**Functional Significance:**
This is function composition in action—building complex operations from simple ones.

## IEnumerable Extensions - LINQ-Style

Extension methods on `IEnumerable<T>` are at the heart of functional programming in C#:

```csharp
public static class EnumerableExtensions
{
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
}
```

**Usage:**
```csharp
var numbers = new List<int> { 1, 5, 10, 15, 20 };
int count = numbers.OrderDescending().CountWhere(n => n > 10);
Console.WriteLine($"Numbers > 10: {count}");  // 2
```

**Key Features:**
- **Generic**: Works with any `IEnumerable<T>`
- **Higher-order function**: Takes a function (`Func<T, bool>`) as parameter
- **Composable**: Chains with other enumerable operations
- **Same pattern as LINQ**: `Where`, `Select`, `OrderBy` are all extension methods

**This is exactly how LINQ works**:
```csharp
// LINQ methods are extension methods!
var result = numbers
    .Where(n => n > 10)      // Extension method
    .Select(n => n * 2)      // Extension method
    .OrderBy(n => n);        // Extension method
```

## Generic Extensions - Maximum Flexibility

Extension methods can be generic, extending any type `T`:

```csharp
public static class GenericExtensions
{
    public static List<T> ToSingleItemList<T>(this T value)
    {
        return new List<T> { value };
    }
}
```

**Usage:**
```csharp
int number = 42;
string text = "hello";
DateTime now = DateTime.Now;

var list1 = number.ToSingleItemList();   // List<int> { 42 }
var list2 = text.ToSingleItemList();     // List<string> { "hello" }
var list3 = now.ToSingleItemList();      // List<DateTime> { <timestamp> }

Console.WriteLine(string.Join(", ", list1));  // 42
Console.WriteLine(string.Join(", ", list2));  // hello
Console.WriteLine(string.Join(", ", list3));  // <timestamp>
```

**Why This is Powerful:**
- Works for **any type** (`int`, `string`, `DateTime`, custom types)
- Single implementation, infinite types
- Gives every type the same "superpower"
- Type-safe: compiler knows `number.ToSingleItemList()` returns `List<int>`

**Functional Use Case:**
Wrapping values is common in functional programming (similar to monads). You might use this to:
- Convert single values to collections
- Adapt APIs that expect collections
- Start functional pipelines

## Functional Programming Patterns

### 1. Pipeline Transformations

```csharp
var result = "hello world"
    .Capitalize()           // "Hello world"
    .Replace("world", "C#") // "Hello C#"
    .ToUpper()              // "HELLO C#"
    .Repeat(2);             // "HELLO C#HELLO C#"
```

Each step transforms the data, passing it to the next operation.

### 2. Higher-Order Functions

Extension methods can take functions as parameters:

```csharp
// Predicate function
numbers.CountWhere(n => n > 10);

// Multiple predicates
var evens = numbers.CountWhere(n => n % 2 == 0);
var odds = numbers.CountWhere(n => n % 2 != 0);
```

This enables generic, reusable logic.

### 3. Fluent APIs

Extension methods enable method chaining for readable, declarative code:

```csharp
var query = numbers
    .Where(n => n > 5)
    .Select(n => n * 2)
    .OrderDescending()
    .Take(10);
```

Reads like natural language: "from numbers, where greater than 5, select doubled, order descending, take 10."

### 4. Extending Third-Party Types

Can't modify `string`, `int`, or `IEnumerable<T>`? Extension methods let you add functionality anyway:

```csharp
// Add methods to built-in types
"text".Capitalize();

// Add methods to library types
myList.CountWhere(x => x.IsValid);

// Add methods to any type
myCustomObject.ToSingleItemList();
```

### 5. Creating Domain-Specific Languages (DSLs)

```csharp
// Extension methods can create fluent DSLs
var request = new HttpRequest()
    .WithMethod("POST")
    .WithUrl("https://api.example.com")
    .WithHeader("Authorization", "Bearer token")
    .WithBody(jsonData);
```

## How LINQ Uses Extension Methods

Every LINQ method is an extension method on `IEnumerable<T>`:

```csharp
// These are all extension methods:
public static IEnumerable<TResult> Select<T, TResult>(
    this IEnumerable<T> source, 
    Func<T, TResult> selector)

public static IEnumerable<T> Where<T>(
    this IEnumerable<T> source, 
    Func<T, bool> predicate)

public static IEnumerable<T> OrderBy<T, TKey>(
    this IEnumerable<T> source, 
    Func<T, TKey> keySelector)
```

**This means you can create your own LINQ-style methods:**

```csharp
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

// Use it like any LINQ method
var every3rd = numbers.TakeEveryNth(3);
```

## Best Practices

### 1. Put Extension Methods in Their Own Namespace

```csharp
namespace MyApp.Extensions
{
    public static class StringExtensions
    {
        // Extension methods here
    }
}
```

Users can import with `using MyApp.Extensions;` only when needed.

### 2. Use Descriptive Names

```csharp
// ✅ Clear
public static string Capitalize(this string value)

// ❌ Unclear
public static string DoIt(this string value)
```

### 3. Check for Null

```csharp
public static string Capitalize(this string value)
{
    if (value == null)
        throw new ArgumentNullException(nameof(value));
    
    // Or handle gracefully
    if (string.IsNullOrEmpty(value))
        return value;
    
    return char.ToUpper(value[0]) + value[1..];
}
```

### 4. Follow Existing Patterns

If extending collections, follow LINQ naming conventions:
- `Where`, `Select`, `First`, `Any`, etc.
- Return `IEnumerable<T>` for chainability
- Use `Func<T, bool>` for predicates

### 5. Don't Overuse

```csharp
// ❌ Not everything needs to be an extension
public static void PrintToConsole(this string value)
{
    Console.WriteLine(value);
}

// ✅ Regular method is fine
public static void Print(string value)
{
    Console.WriteLine(value);
}
```

Use extension methods when they genuinely enhance the type's API or enable fluent chaining.

## Extension Methods vs. Inheritance

| Extension Methods | Inheritance |
|-------------------|-------------|
| Non-invasive | Requires source code access |
| Works on sealed types | Cannot extend sealed types |
| Static binding (compile-time) | Dynamic binding (runtime) |
| Can extend interfaces | Must implement interface |
| No access to private members | Access to protected members |
| Cannot override instance methods | Can override virtual methods |

**When to use extension methods:**
- Extending types you don't control
- Adding utility methods
- Creating fluent APIs
- Functional-style operations

**When to use inheritance:**
- Need polymorphism
- Core functionality of the type
- Access to protected members required

## Resolution and Scoping

Extension methods are resolved at **compile time** based on imported namespaces:

```csharp
using System.Linq;  // Brings LINQ extension methods into scope

var numbers = new[] { 1, 2, 3 };
var doubled = numbers.Select(x => x * 2);  // Can use Select
```

Without the `using`, extension methods aren't available:

```csharp
// Without: using System.Linq;
var doubled = numbers.Select(x => x * 2);  // Compile error!

// Can still call as static method
var doubled = Enumerable.Select(numbers, x => x * 2);  // Works
```

## Common Patterns in Functional Programming

### Map/Select Pattern

```csharp
public static IEnumerable<TResult> Map<T, TResult>(
    this IEnumerable<T> source,
    Func<T, TResult> mapper)
{
    foreach (var item in source)
        yield return mapper(item);
}

// Usage
var doubled = numbers.Map(x => x * 2);
```

### Filter/Where Pattern

```csharp
public static IEnumerable<T> Filter<T>(
    this IEnumerable<T> source,
    Func<T, bool> predicate)
{
    foreach (var item in source)
        if (predicate(item))
            yield return item;
}

// Usage
var evens = numbers.Filter(x => x % 2 == 0);
```

### Tap/Tee Pattern (Side Effects)

```csharp
public static T Tap<T>(this T value, Action<T> action)
{
    action(value);
    return value;
}

// Usage - inject debugging without breaking chain
var result = numbers
    .Where(x => x > 10)
    .Tap(x => Console.WriteLine($"After filter: {x.Count()}"))
    .Select(x => x * 2);
```

### Pipe Pattern

```csharp
public static TResult Pipe<T, TResult>(this T value, Func<T, TResult> func)
    => func(value);

// Usage
var result = "hello"
    .Pipe(s => s.ToUpper())
    .Pipe(s => s.Replace("E", "3"));
```

## Conclusion

The examples in [Extensions.cs](Lesson02/Extensions.cs) demonstrate:

1. **String extensions** (`Capitalize`, `Repeat`) - Adding functionality to built-in types
2. **Method chaining** - Composing operations fluently
3. **IEnumerable extensions** (`CountWhere`) - LINQ-style higher-order functions
4. **Generic extensions** (`ToSingleItemList`) - Extending any type with generic methods

Extension methods are fundamental to functional programming in C# because they:

- ✅ **Enable fluent APIs**: Chain operations naturally
- ✅ **Support higher-order functions**: Pass functions as parameters
- ✅ **Create pipelines**: Transform data through stages
- ✅ **Extend any type**: Add functionality without modification
- ✅ **Power LINQ**: All LINQ methods are extension methods
- ✅ **Promote immutability**: Return new values rather than modify

Understanding extension methods is essential for:
- Writing idiomatic C# code
- Using and creating LINQ-style operations
- Building fluent, composable APIs
- Applying functional programming patterns
- Creating domain-specific languages

Extension methods transform C# from a purely object-oriented language into one that elegantly supports functional programming paradigms, making code more readable, maintainable, and expressive.
