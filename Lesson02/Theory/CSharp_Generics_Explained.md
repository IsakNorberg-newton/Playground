# C# Generics: A Functional Programming Perspective

## Introduction

Generics in C# enable you to write reusable, type-safe code that works with any data type while maintaining compile-time type checking. Introduced in C# 2.0, generics are fundamental to functional programming, providing **parametric polymorphism**—the ability to write code that works uniformly across all types.

## Why Generics Matter in Functional Programming

Generics provide:
1. **Type safety**: Catch errors at compile time, not runtime
2. **Performance**: Elimination of boxing/unboxing overhead
3. **Reusability**: Write code once, use with any type
4. **Abstraction**: Generic algorithms that work across types

## Simple Generic Class - Box&lt;T&gt;

```csharp
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
```

**Usage:**
```csharp
var intBox = new Box<int>(42);
var stringBox = new Box<string>("Hello Generics");
var dateBox = new Box<DateTime>(DateTime.Now);

Console.WriteLine(intBox);     // Box contains: 42
Console.WriteLine(stringBox);  // Box contains: Hello Generics
Console.WriteLine(dateBox);    // Box contains: 12/20/2025 ...
```

**Key Points:**
- `T` is a **type parameter** - a placeholder for any type
- The same class works for `int`, `string`, `DateTime`, or any other type
- Type safety: `intBox.GetContent()` returns `int`, not `object`
- No boxing/unboxing for value types (performance benefit)
- Compile-time type checking prevents type errors

**Functional Significance:**
This is **parametric polymorphism**—a type that works uniformly across all types, allowing you to write generic, reusable code.

## Multiple Type Parameters - Pair&lt;TFirst, TSecond&gt;

```csharp
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
```

**Usage:**
```csharp
var pair1 = new Pair<string, int>("Age", 25);
var pair2 = new Pair<int, double>(1, 3.14);

Console.WriteLine(pair1);  // (Age, 25)
Console.WriteLine(pair2);  // (1, 3.14)
```

**Key Points:**
- Two independent type parameters
- First and Second can be different types
- Useful for grouping related data without creating a custom class

## Generic Constraints - Repository&lt;T&gt;

Constraints limit which types can be used with a generic, enabling type-specific operations:

```csharp
public class Repository<T> where T : class, new()
{
    private List<T> _items = new();

    public void Add(T item) => _items.Add(item);
    public IEnumerable<T> GetAll() => _items;
    public T CreateNew() => new T();
    public int Count => _items.Count;
}
```

**Constraints Applied:**
- `where T : class` - T must be a reference type
- `new()` - T must have a parameterless constructor

**Usage:**
```csharp
var personRepo = new Repository<Employee>();
personRepo.Add(new Employee(...));
personRepo.Add(new Employee(...));

Console.WriteLine($"Repository count: {personRepo.Count}");
foreach (var person in personRepo.GetAll())
{
    Console.WriteLine($"- {person}");
}

// Can create new instances
var newEmployee = personRepo.CreateNew();
```

**Why Constraints Matter:**
- `class` constraint: Ensures T is a reference type (can be null)
- `new()` constraint: Enables creating new instances with `new T()`
- Constraints allow you to call specific methods or operations on the generic type
- Balance between flexibility and functionality

## Built-in Generic Collections

## Built-in Generic Collections

C# provides rich generic collection types that work with any type:

### List&lt;T&gt;

```csharp
var list = new List<int> { 1, 2, 3, 4, 5 };
Console.WriteLine(string.Join(", ", list));  // 1, 2, 3, 4, 5
```

**Characteristics:**
- Ordered, indexed collection
- Resizable array
- Fast random access

### Dictionary&lt;TKey, TValue&gt;

```csharp
var dict = new Dictionary<string, int>
{
    ["one"] = 1,
    ["two"] = 2,
    ["three"] = 3
};

Console.WriteLine(string.Join(", ", 
    dict.Select(kv => $"{kv.Key}={kv.Value}")));
// one=1, two=2, three=3
```

**Characteristics:**
- Key-value pairs
- Fast lookup by key
- Functional equivalent of "maps" in other languages
- Essential for memoization and caching

### HashSet&lt;T&gt;

```csharp
var set = new HashSet<string> { "apple", "banana", "cherry", "apple" };
Console.WriteLine(string.Join(", ", set));  // apple, banana, cherry (no duplicate)
```

**Characteristics:**
- Unordered collection
- Automatically removes duplicates
- Fast membership testing

## Why Generics Matter

### Type Safety

```csharp
// ❌ Without generics (old ArrayList)
ArrayList list = new ArrayList();
list.Add(42);
list.Add("text");
int value = (int)list[1];  // Runtime exception!

// ✅ With generics
List<int> list = new List<int>();
list.Add(42);
// list.Add("text");  // Compile error - caught early!
int value = list[0];  // No cast needed, type-safe
```

Generics catch type errors at **compile time**, not runtime.

### Performance - No Boxing/Unboxing

```csharp
// ❌ Boxing with ArrayList
ArrayList list = new ArrayList();
list.Add(42);  // Boxing: int → object (heap allocation)
int value = (int)list[0];  // Unboxing

// ✅ No boxing with List<T>
List<int> list = new List<int>();
list.Add(42);  // No boxing, stays as int
int value = list[0];  // No unboxing, direct access
```

Value types in generic collections avoid boxing overhead, improving performance.

### Code Reuse

Instead of writing separate classes for each type:

```csharp
// ❌ Without generics
IntBox, StringBox, DateTimeBox...

// ✅ With generics - write once, use everywhere
Box<int>, Box<string>, Box<DateTime>...
```

One generic class works for all types.

## Generics in Functional Programming

Generics enable **parametric polymorphism** - writing functions and types that work uniformly across all types. This is fundamental to functional programming.

**Key Benefits:**
- Write generic algorithms that work for any type
- Type-safe transformations (LINQ)
- Composable, reusable components
- Foundation for functional patterns

## Naming Conventions

```csharp
// Standard conventions:
T        // Single type parameter
TKey     // Key type
TValue   // Value type  
TResult  // Result type
TFirst, TSecond  // Multiple related types
```

## Conclusion

The examples in [GenericsExamples.cs](Lesson02/GenericsExamples.cs) demonstrate the core benefits of C# generics:

1. **`Box<T>`** - Simple generic container showing parametric polymorphism
2. **`Pair<TFirst, TSecond>`** - Multiple type parameters for grouping data
3. **`Repository<T>`** - Constraints enable type-specific operations
4. **Built-in collections** - `List<T>`, `Dictionary<TKey, TValue>`, `HashSet<T>` work with any type

Generics provide:
- ✅ **Type safety** - Compile-time error checking
- ✅ **Performance** - No boxing/unboxing for value types
- ✅ **Reusability** - Write once, use with any type
- ✅ **Abstraction** - Generic algorithms and data structures

Understanding generics is essential for functional programming in C#, as they form the foundation of LINQ and enable writing type-safe, reusable, and composable code.

