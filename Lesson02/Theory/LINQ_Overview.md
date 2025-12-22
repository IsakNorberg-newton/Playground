# LINQ: Language Integrated Query

## What is LINQ?

LINQ (Language Integrated Query) is a powerful feature in C# that brings query capabilities directly into the language. It provides a consistent, declarative way to query and transform data from various sources—collections, databases, XML, and more—using a functional programming approach.

## Why LINQ Matters in Functional Programming

LINQ is functional programming in C#. It embodies core functional principles:

1. **Declarative**: Describe *what* you want, not *how* to get it
2. **Composable**: Chain operations to build complex queries
3. **Lazy Evaluation**: Queries execute only when enumerated
4. **Immutable**: Original collections never modified
5. **Higher-Order Functions**: Operations take and return functions

## LINQ Syntax

LINQ offers two syntaxes:

### Query Syntax (SQL-like)
```csharp
var result = from item in collection
             where item > 5
             select item * 2;
```

### Method Syntax (Fluent)
```csharp
var result = collection
    .Where(item => item > 5)
    .Select(item => item * 2);
```

Both are equivalent. Method syntax is more common and flexible.

## How LINQ Works

LINQ is built entirely on:
- **Extension methods** on `IEnumerable<T>` and `IQueryable<T>`
- **Generic types** for type safety
- **Lambda expressions** for inline functions
- **Deferred execution** for lazy evaluation

## Core LINQ Operations

LINQ operations fall into several categories:

### 1. Filtering
Select elements that match criteria
- `Where` - Filter by condition

### 2. Projection
Transform elements into new shapes
- `Select` - Transform each element
- `SelectMany` - Flatten nested collections

### 3. Ordering
Sort elements
- `OrderBy`, `OrderByDescending` - Sort by key
- `ThenBy`, `ThenByDescending` - Secondary sort

### 4. Grouping
Group elements by key
- `GroupBy` - Create groups

### 5. Joining
Combine multiple sources
- `Join` - Inner join
- `GroupJoin` - Left outer join

### 6. Aggregation
Compute single values from collections
- `Count`, `Sum`, `Average`, `Min`, `Max`
- `Aggregate` - Custom aggregation

### 7. Set Operations
Set-based operations
- `Distinct`, `Union`, `Intersect`, `Except`

### 8. Element Operations
Get specific elements
- `First`, `Last`, `Single`, `ElementAt`
- `FirstOrDefault`, `LastOrDefault`, etc.

### 9. Quantifiers
Test conditions on elements
- `Any`, `All`, `Contains`

### 10. Generation
Create sequences
- `Range`, `Repeat`, `Empty`

### 11. Conversion
Convert between types
- `ToList`, `ToArray`, `ToDictionary`

## Deferred Execution

Most LINQ operations use **deferred execution**—the query doesn't run until you iterate:

```csharp
var query = numbers.Where(n => n > 5);  // Query defined, not executed

foreach (var n in query)  // NOW it executes
{
    Console.WriteLine(n);
}
```

**Benefits:**
- Build complex queries step by step
- Query executes with current data
- Memory efficient

**Immediate execution** methods force evaluation:
- `ToList()`, `ToArray()`, `Count()`, `First()`, etc.

## Functional Programming with LINQ

LINQ enables functional programming patterns:

### Immutability
```csharp
var original = new[] { 1, 2, 3 };
var doubled = original.Select(x => x * 2);
// original is unchanged
```

### Composition
```csharp
var result = numbers
    .Where(n => n > 0)
    .Select(n => n * 2)
    .OrderBy(n => n)
    .Take(10);
```

### Higher-Order Functions
```csharp
// Functions that take functions
numbers.Where(n => n > 5)
numbers.Select(n => n * 2)
numbers.OrderBy(n => n.Length)
```

### Pipelining
```csharp
// Data flows through transformation pipeline
var result = GetData()
    .Filter()
    .Transform()
    .Sort()
    .Format();
```

## LINQ Example Categories

The examples in the Linq folder demonstrate all LINQ capabilities:

1. **LinqFiltering** - `Where` to filter elements
2. **LinqProjecting** - `Select`, `SelectMany` for transformations
3. **LinqOrdering** - `OrderBy`, `ThenBy` for sorting
4. **LinqGrouping** - `GroupBy` for grouping
5. **LinqJoining** - `Join`, `GroupJoin` for combining data
6. **LinqAggregation** - `Count`, `Sum`, `Average`, `Min`, `Max`, `Aggregate`
7. **LinqSetOps** - `Distinct`, `Union`, `Intersect`, `Except`
8. **LinqElementOps** - `First`, `Last`, `Single`, `ElementAt`
9. **LinqQuantifier** - `Any`, `All`, `Contains`
10. **LinqGeneration** - `Range`, `Repeat`, `Empty`
11. **LinqConversionExport** - `ToList`, `ToArray`, `ToDictionary`
12. **LinqConversionImport** - `Cast`, `OfType`

## Why LINQ is Revolutionary

Before LINQ, data querying required:
- Different syntax for different data sources (SQL, objects, XML)
- Imperative loops and conditionals
- Verbose, error-prone code
- String-based queries (no compile-time checking)

With LINQ:
- ✅ **Unified syntax** across all data sources
- ✅ **Type-safe** queries with IntelliSense
- ✅ **Declarative** and readable
- ✅ **Composable** and testable
- ✅ **Functional** programming paradigm

## Conclusion

LINQ is the cornerstone of functional programming in C#. It transforms data processing from imperative loops into declarative, composable queries. Every LINQ operation is an extension method that returns an enumerable, enabling infinite chaining and composition.

The following documentation files explore each LINQ category in detail, showing how functional programming principles are applied to real-world data processing scenarios.
