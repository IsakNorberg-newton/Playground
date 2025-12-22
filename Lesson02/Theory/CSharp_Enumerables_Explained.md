# C# Enumerables: A Functional Programming Perspective

## Introduction

`IEnumerable<T>` is one of the most important interfaces in C# and the foundation of functional programming in .NET. It represents a sequence of elements that can be iterated over, enabling lazy evaluation, composability, and declarative data processing—all core principles of functional programming.

## Why Enumerables Matter in Functional Programming

Enumerables provide:
1. **Lazy Evaluation**: Elements are generated on-demand, not all at once
2. **Composability**: Chain operations together to create data processing pipelines
3. **Abstraction**: Uniform interface for all collections and sequences
4. **Memory Efficiency**: Process large or infinite sequences without loading everything into memory

## Basic Enumeration - Built-in Types

All collections, arrays, and strings in .NET implement `IEnumerable<T>`:

### Arrays

```csharp
var array = new int[] { 1, 2, 3, 4, 5 };
foreach (var item in array)
{
    Console.WriteLine(item); // 1, 2, 3, 4, 5
}
```

### Lists

```csharp
var list = new List<int> { 1, 2, 3 };
foreach (var item in list)
{
    Console.WriteLine(item); // 1, 2, 3
}
```

### Strings

```csharp
foreach (char c in "beer")
{
    Console.WriteLine(c); // b, e, e, r
}
```

**Key Point**: The `foreach` loop works with any type that implements `IEnumerable<T>`.

## How Enumeration Works - The Enumerator Pattern

Behind every `foreach` loop is an **enumerator**—an object that knows how to iterate through a collection:

```csharp
using (var enumerator = "beer".GetEnumerator())
    while (enumerator.MoveNext())
    {
        var element = enumerator.Current;
        Console.WriteLine(element); // b, e, e, r
    }
```

**What's Happening:**
1. `GetEnumerator()` returns an `IEnumerator<T>` object
2. `MoveNext()` advances to the next element (returns `true` if successful)
3. `Current` property gets the current element
4. Loop continues until `MoveNext()` returns `false`

This is what the compiler generates for every `foreach` loop—it's just syntactic sugar.

## The IEnumerable Interface

```csharp
public interface IEnumerable<T>
{
    IEnumerator<T> GetEnumerator();
}
```

```csharp
public interface IEnumerator<T>
{
    T Current { get; }
    bool MoveNext();
    void Reset();
    void Dispose();
}
```

**Two Key Interfaces:**
- `IEnumerable<T>` - Represents a sequence (the collection)
- `IEnumerator<T>` - Represents the iterator (current position in the sequence)

## Creating Enumerables

There are four ways to create custom enumerables, demonstrated in [EnumerableImplementation.cs](Lesson02/EnumerableImplementation.cs):

### 1. Simple Method with `yield return`

The easiest and most common way:

```csharp
public static IEnumerable<string> MethodExample()
{
    yield return "Good morning";
    yield return "Good afternoon";
    yield return "Good evening";
}
```

**Usage:**
```csharp
foreach (var item in MethodExample())
    Console.WriteLine($"Method: {item}");
// Output:
// Method: Good morning
// Method: Good afternoon
// Method: Good evening
```

**Why This is Powerful:**
- No need to create a class
- Compiler generates the enumerator automatically
- Lazy evaluation: values produced on-demand
- Can use loops and conditionals inside

### 2. Iterator Class with `yield return`

Implement `IEnumerable<T>` with `yield return`:

```csharp
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
```

**Usage:**
```csharp
foreach (var x in new IteratorExample())
    Console.WriteLine($"Iterator: {x}");
// Output:
// Iterator: 1
// Iterator: 2
// Iterator: 3
```

**Key Points:**
- Class implements `IEnumerable<T>`
- `GetEnumerator()` uses `yield return` to produce values
- Non-generic `GetEnumerator()` required for backward compatibility

### 3. Wrapping an Existing Enumerable

Delegate to another collection's enumerator:

```csharp
public class WrappedEnumerable : IEnumerable<int>
{
    private readonly List<int> _data = new() { 1, 2, 3 };

    public IEnumerator<int> GetEnumerator() => _data.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
```

**Usage:**
```csharp
foreach (var x in new WrappedEnumerable())
    Console.WriteLine($"Wrapped: {x}");
// Output:
// Wrapped: 1
// Wrapped: 2
// Wrapped: 3
```

**When to Use:**
- Wrapping existing collections with additional behavior
- Encapsulating data access
- Implementing custom collection classes

### 4. Manual Implementation

Full control by implementing both interfaces manually:

```csharp
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
```

**Usage:**
```csharp
foreach (var x in new ManualEnumerable())
    Console.WriteLine($"Manual: {x}");
// Output:
// Manual: 1
// Manual: 2
// Manual: 3
```

**Components:**
- `_index`: Tracks current position
- `Current`: Returns element at current position
- `MoveNext()`: Advances to next element, returns `false` when done
- `Reset()`: Resets to beginning (rarely used)
- `Dispose()`: Cleanup resources (empty in this simple example)

**When to Use:**
- Need precise control over iteration
- Complex iteration logic
- Performance-critical scenarios
- Generally prefer `yield return` for simplicity

## Comparison of Approaches

| Approach | Complexity | Use Case | State Management |
|----------|-----------|----------|------------------|
| **Method with `yield`** | Lowest | Simple sequences | Automatic |
| **Iterator class with `yield`** | Low | Reusable iterators | Automatic |
| **Wrapped enumerable** | Low | Delegate to existing collection | None needed |
| **Manual implementation** | High | Complex iteration logic | Manual |

## Lazy Evaluation

One of the most powerful features of enumerables is **lazy evaluation**:

```csharp
public static IEnumerable<string> MethodExample()
{
    Console.WriteLine("First yield");
    yield return "Good morning";
    
    Console.WriteLine("Second yield");
    yield return "Good afternoon";
    
    Console.WriteLine("Third yield");
    yield return "Good evening";
}
```

When you call `MethodExample()`, **nothing executes yet**. The code only runs when you iterate:

```csharp
var greetings = MethodExample(); // No console output yet!

foreach (var greeting in greetings)
{
    Console.WriteLine(greeting);
}
// Now you'll see:
// First yield
// Good morning
// Second yield
// Good afternoon
// Third yield
// Good evening
```

**Why This Matters:**
- Values generated on-demand (pull model)
- Can represent infinite sequences
- Memory efficient—only current element in memory
- Enables composable pipelines (LINQ)

## Enumerables in Functional Programming

### 1. Sequences as First-Class Values

Enumerables treat sequences as values that can be passed around:

```csharp
IEnumerable<string> GetGreetings() => MethodExample();

var greetings = GetGreetings();
// Pass to other functions, return from functions, store in variables
```

### 2. Composability

Enumerables are the foundation of LINQ, enabling functional composition:

```csharp
var numbers = Enumerable.Range(1, 10);

var result = numbers
    .Where(x => x % 2 == 0)
    .Select(x => x * x)
    .Take(3);

foreach (var n in result)
    Console.WriteLine(n);  // 4, 16, 36
```

Each operation returns a new `IEnumerable<T>`, allowing chaining.

### 3. Separation of Definition and Execution

```csharp
// Define the sequence
var query = numbers.Where(x => x > 5);

// Execute when needed (potentially multiple times)
foreach (var n in query) { /* ... */ }
foreach (var n in query) { /* ... */ }  // Re-executes
```

This separation enables:
- Reusable queries
- Deferred execution
- Flexible data processing

### 4. Immutability

Enumerables don't modify the source—they create new sequences:

```csharp
var original = new[] { 1, 2, 3 };
var doubled = original.Select(x => x * 2);

// original is unchanged
// doubled is a new sequence
```

### 5. Infinite Sequences

Lazy evaluation enables infinite sequences:

```csharp
public static IEnumerable<int> InfiniteNumbers()
{
    int i = 0;
    while (true)
    {
        yield return i++;
    }
}

var firstTen = InfiniteNumbers().Take(10);
```

Without lazy evaluation, this would never work.

## The Power of `yield return`

The `yield` keyword is syntactic sugar that generates a state machine:

**What you write:**
```csharp
public static IEnumerable<int> GetNumbers()
{
    yield return 1;
    yield return 2;
    yield return 3;
}
```

**What the compiler generates:**
- A class implementing `IEnumerator<int>`
- State tracking for the current position
- `MoveNext()` method with a switch statement
- Automatic state transitions

This eliminates the need to manually implement the enumerator pattern in most cases.

## Key Concepts Summary

### IEnumerable&lt;T&gt;
- Represents a **sequence** of elements
- **Pull-based**: Consumers request elements when needed
- **Lazy**: Elements generated on-demand
- **Composable**: Chain operations together
- **Foundation** of LINQ and functional programming in C#

### foreach Loop
- Syntactic sugar over the enumerator pattern
- Calls `GetEnumerator()`, then `MoveNext()` repeatedly
- Automatically disposes the enumerator

### yield return
- Generates enumerators automatically
- Maintains state between calls
- Enables lazy evaluation
- Simplest way to create custom enumerables

### Manual Implementation
- Full control over iteration behavior
- Implement `IEnumerable<T>` and `IEnumerator<T>`
- More complex but more flexible

## Functional Programming Benefits

Enumerables enable functional programming in C# by providing:

1. **Declarative Style**: Describe *what* you want, not *how* to get it
2. **Immutability**: Original sequences never modified
3. **Lazy Evaluation**: Compute only what's needed
4. **Composability**: Build complex operations from simple ones
5. **Abstraction**: Uniform interface for all sequences
6. **Pipelining**: Chain operations naturally

## Conclusion

The examples in [EnumerableExamples.cs](Lesson02/EnumerableExamples.cs) and [EnumerableImplementation.cs](Lesson02/EnumerableImplementation.cs) demonstrate:

1. **Built-in support**: Arrays, lists, and strings are enumerable
2. **Simple enumeration**: Using `foreach` for iteration
3. **Explicit enumeration**: Using `GetEnumerator()` and `MoveNext()`
4. **Four ways to create enumerables**:
   - Method with `yield return` (simplest)
   - Iterator class with `yield return`
   - Wrapping existing enumerables
   - Manual implementation (most control)

`IEnumerable<T>` is the cornerstone of functional programming in C#:
- Enables LINQ (Language Integrated Query)
- Provides lazy, composable sequences
- Supports infinite and generated sequences
- Separates sequence definition from execution
- Forms the basis of functional data pipelines

Understanding enumerables is essential for writing functional, efficient, and expressive C# code. They transform data processing from imperative loops into declarative, composable operations that are easier to read, test, and maintain.
