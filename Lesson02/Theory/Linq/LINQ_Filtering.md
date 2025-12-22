# LINQ Filtering Operations

## Overview

Filtering is one of the most fundamental LINQ operations. It allows you to select only the elements from a sequence that satisfy specific criteria. In functional programming, filtering is a core operation that transforms collections without modifying the original data.

## The Where Operation

### Basic Filtering

```csharp
var vets = employees.Where(e => e.Role == WorkRole.Veterinarian);
```

**What it does:**
- Filters employees to only those who are veterinarians
- Takes a predicate function (`Func<Employee, bool>`)
- Returns `IEnumerable<Employee>` containing only matching elements

**Functional Characteristics:**
- **Non-destructive**: Original collection unchanged
- **Lazy evaluation**: Query doesn't execute until enumerated
- **Composable**: Can chain with other operations

### How Where Works

Behind the scenes:
```csharp
public static IEnumerable<T> Where<T>(this IEnumerable<T> source, Func<T, bool> predicate)
{
    foreach (var item in source)
    {
        if (predicate(item))
            yield return item;
    }
}
```

The predicate is a **higher-order function**—a function that takes another function as a parameter.

## Take and Skip Operations

### Take - Get First N Elements

```csharp
var first5 = employees.Take(5);
System.Console.WriteLine("First 5 employees:");
first5.ToList().ForEach(e => System.Console.WriteLine($"{e.FirstName} {e.LastName}"));
```

**What it does:**
- Returns the first 5 elements from the sequence
- Useful for pagination or limiting results
- Stops enumeration after N items (efficient)

**Use Cases:**
- Top N results
- Preview/sample data
- Pagination (first page)

### Skip - Bypass First N Elements

```csharp
var afterFirst10 = employees.Skip(10);
System.Console.WriteLine("Employees after skipping first 10 (showing 5):");
afterFirst10.Take(5).ToList().ForEach(e => System.Console.WriteLine($"{e.FirstName} {e.LastName}"));
```

**What it does:**
- Skips the first 10 elements
- Returns all remaining elements
- Combined with `Take` for pagination: `Skip(10).Take(5)` gets items 11-15

**Pagination Pattern:**
```csharp
int pageSize = 10;
int pageNumber = 2; // 0-based
var page = employees.Skip(pageNumber * pageSize).Take(pageSize);
```

## Conditional Take and Skip

### TakeWhile - Take Until Condition Fails

```csharp
var orderedByHire = employees.OrderBy(e => e.HireDate);
var hiredBefore2010 = orderedByHire.TakeWhile(e => e.HireDate.Year < 2010);
System.Console.WriteLine("Employees hired before 2010 (TakeWhile on ordered list):");
hiredBefore2010.ToList().ForEach(e => System.Console.WriteLine($"{e.FirstName} {e.LastName} - {e.HireDate:d}"));
```

**What it does:**
- Takes elements **while** the condition is true
- **Stops immediately** when first false encountered
- Order matters! Works best on sorted data

**Important Note:**
`TakeWhile` stops at the **first failure**, unlike `Where`:
```csharp
var numbers = new[] { 1, 3, 5, 2, 7, 9 };
var takeWhile = numbers.TakeWhile(n => n < 5);  // [1, 3] - stops at 5
var where = numbers.Where(n => n < 5);          // [1, 3, 2] - checks all
```

### SkipWhile - Skip Until Condition Fails

```csharp
var skipUntilVet = employees.SkipWhile(e => e.Role != WorkRole.Veterinarian);
System.Console.WriteLine("After SkipWhile until first Veterinarian (showing 5):");
skipUntilVet.Take(5).ToList().ForEach(e => System.Console.WriteLine($"{e.FirstName} {e.LastName} - {e.Role}"));
```

**What it does:**
- Skips elements **while** the condition is true
- **Starts returning** elements once condition becomes false
- Returns all remaining elements after that point

**Pattern:**
```csharp
// Skip until we find what we're looking for, then take the rest
var afterMarker = items.SkipWhile(item => item != marker);
```

## Comparison of Filtering Operations

| Operation | Behavior | Use Case |
|-----------|----------|----------|
| `Where` | Tests every element | General filtering |
| `Take(n)` | First n elements | Limiting results |
| `Skip(n)` | All except first n | Pagination offset |
| `TakeWhile` | While condition true | Contiguous prefix |
| `SkipWhile` | While condition true, then all | Skip prefix |

## Functional Programming Concepts

### 1. Declarative Style

**Imperative (traditional):**
```csharp
var vets = new List<Employee>();
foreach (var employee in employees)
{
    if (employee.Role == WorkRole.Veterinarian)
    {
        vets.Add(employee);
    }
}
```

**Declarative (LINQ):**
```csharp
var vets = employees.Where(e => e.Role == WorkRole.Veterinarian);
```

LINQ expresses *what* you want, not *how* to get it.

### 2. Composition

Filtering operations chain naturally:

```csharp
var result = employees
    .Where(e => e.Role == WorkRole.Veterinarian)  // Filter by role
    .Where(e => e.HireDate.Year >= 2020)          // Filter by date
    .Take(10);                                     // Limit results
```

Each operation returns `IEnumerable<T>`, enabling endless composition.

### 3. Lazy Evaluation

```csharp
var vets = employees.Where(e => e.Role == WorkRole.Veterinarian);
// Query defined but NOT executed yet

foreach (var vet in vets)  // NOW it executes
{
    Console.WriteLine(vet.FirstName);
}
```

Benefits:
- Build complex queries step by step
- Query executes with current data
- Memory efficient (no intermediate collections)

### 4. Immutability

```csharp
var original = employees;
var filtered = employees.Where(e => e.Role == WorkRole.Veterinarian);

// original is unchanged - filtering creates a new view
```

## Practical Patterns

### Pagination

```csharp
public IEnumerable<Employee> GetPage(int pageNumber, int pageSize)
{
    return employees
        .Skip(pageNumber * pageSize)
        .Take(pageSize);
}
```

### Conditional Filtering

```csharp
var query = employees.AsEnumerable();

if (filterByRole)
    query = query.Where(e => e.Role == targetRole);

if (filterByDate)
    query = query.Where(e => e.HireDate >= startDate);

return query;
```

### Ordered Take/Skip

```csharp
// Get employees 11-20 by hire date
var recent = employees
    .OrderByDescending(e => e.HireDate)
    .Skip(10)
    .Take(10);
```

## Performance Considerations

### Where
- **O(n)**: Must check every element
- **Lazy**: Only processes elements as needed
- **No allocation**: Uses iterator pattern

### Take
- **O(n)**: But stops after n elements
- **Efficient**: Doesn't process remaining elements

### Skip
- **O(n)**: Must iterate through skipped elements
- For large skips, consider other data structures

### TakeWhile/SkipWhile
- **O(k)** where k is elements until condition fails
- **Order dependent**: Results vary based on sequence order

## Common Mistakes

### 1. TakeWhile vs Where

```csharp
var numbers = new[] { 1, 5, 2, 8, 3, 9 };

// ❌ Wrong - TakeWhile stops at 8
var takeWhile = numbers.TakeWhile(n => n < 5);  // [1]

// ✅ Correct - Where checks all
var where = numbers.Where(n => n < 5);  // [1, 2, 3]
```

### 2. Forgetting to Enumerate

```csharp
// ❌ Query not executed
var filtered = employees.Where(e => e.Role == WorkRole.Veterinarian);

// ✅ Force execution
var filtered = employees.Where(e => e.Role == WorkRole.Veterinarian).ToList();
```

### 3. Multiple Enumeration

```csharp
var query = employees.Where(e => e.Role == WorkRole.Veterinarian);

// ❌ Executes query twice (if employees is queryable)
Console.WriteLine(query.Count());
Console.WriteLine(query.First());

// ✅ Execute once, store result
var results = query.ToList();
Console.WriteLine(results.Count);
Console.WriteLine(results.First());
```

## Conclusion

Filtering operations in LINQ demonstrate core functional programming principles:

- ✅ **Declarative**: Express intent clearly
- ✅ **Composable**: Chain operations naturally
- ✅ **Lazy**: Execute only when needed
- ✅ **Immutable**: Original data unchanged
- ✅ **Higher-order functions**: Functions take functions

The examples in [LinqFiltering.cs](LinqFiltering.cs) show:
- `Where` for conditional filtering
- `Take`/`Skip` for limiting and offsetting
- `TakeWhile`/`SkipWhile` for conditional sequences

These operations are the foundation of data processing in functional C#, enabling readable, maintainable queries that clearly express business logic.
