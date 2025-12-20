# LINQ Element Operations

## Overview

Element operations in LINQ retrieve specific elements from sequences based on position or criteria. These operations short-circuit when possible and provide safe ways to access sequence elements. They're essential for extracting single values from collections in a functional, declarative manner.

## First and FirstOrDefault

### First - Expects Element

```csharp
var first = employees.First();
System.Console.WriteLine($"First employee: {first.FirstName} {first.LastName}");
```

**What it does:**
- Returns the **first** element
- **Throws** `InvalidOperationException` if sequence is empty
- Short-circuits (stops after finding first)

**Signature:**
```csharp
TSource First<TSource>(this IEnumerable<TSource> source)
TSource First<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
```

**With predicate:**
```csharp
var firstManager = employees.First(e => e.Role == WorkRole.Management);
```

### FirstOrDefault - Safe Alternative

```csharp
var firstOrDefault = employees.FirstOrDefault(e => e.Role == WorkRole.Management);
System.Console.WriteLine(firstOrDefault is not null
    ? $"FirstOrDefault (Management): {firstOrDefault.FirstName} {firstOrDefault.LastName}"
    : "FirstOrDefault (Management): <none>");
```

**What it does:**
- Returns first element or `default(T)` if none found
- `default(T)` = `null` for reference types, `0` for numbers, `false` for bool
- **Never throws** for empty sequence

**When to use:**
- Use `First()` when you're **certain** element exists
- Use `FirstOrDefault()` when element **might not** exist

## Last and LastOrDefault

### LastOrDefault - From End

```csharp
var lastHired = employees.OrderBy(e => e.HireDate).LastOrDefault();
System.Console.WriteLine(lastHired is not null
    ? $"LastOrDefault (by hire date): {lastHired.FirstName} {lastHired.LastName} - {lastHired.HireDate:d}"
    : "LastOrDefault: <none>");
```

**What it does:**
- Returns the **last** element
- `LastOrDefault()` returns `default(T)` if empty
- `Last()` throws if empty

**Signature:**
```csharp
TSource Last<TSource>(this IEnumerable<TSource> source)
TSource LastOrDefault<TSource>(this IEnumerable<TSource> source)
TSource Last<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
TSource LastOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
```

**Performance note:**
- For `IList<T>`: O(1) - direct index access
- For `IEnumerable<T>`: O(n) - must enumerate entire sequence

**Example shows ordering:**
```csharp
// Makes result deterministic by ordering first
var lastHired = employees.OrderBy(e => e.HireDate).LastOrDefault();
```

## Single and SingleOrDefault

### Single - Exactly One

```csharp
var singleId = employees.Select(e => e.EmployeeId).FirstOrDefault();
var singleEmp = employees.Where(e => e.EmployeeId == singleId).Single();
System.Console.WriteLine($"Single by EmployeeId (existing): {singleEmp.FirstName} {singleEmp.LastName}");
```

**What it does:**
- Returns element if **exactly one** matches
- **Throws** if:
  - Zero elements (no match)
  - More than one element (multiple matches)
- Use when uniqueness is required

**Signature:**
```csharp
TSource Single<TSource>(this IEnumerable<TSource> source)
TSource Single<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
```

**Exceptions:**
```csharp
// Throws InvalidOperationException
var single = emptySequence.Single();  // "Sequence contains no elements"
var single = multipleMatches.Single();  // "Sequence contains more than one element"
```

### SingleOrDefault - Exactly One or None

```csharp
var missingId = Guid.NewGuid();
var singleOrDefault = employees.Where(e => e.EmployeeId == missingId).SingleOrDefault();
System.Console.WriteLine(singleOrDefault is null 
    ? "SingleOrDefault (missing): <null>" 
    : "SingleOrDefault returned a value");
```

**What it does:**
- Returns element if exactly one matches
- Returns `default(T)` if zero matches
- **Still throws** if more than one match

**Comparison:**
| Method | 0 matches | 1 match | 2+ matches |
|--------|-----------|---------|------------|
| `Single` | Throws | Returns | Throws |
| `SingleOrDefault` | Returns default | Returns | Throws |

**Use cases:**
```csharp
// When ID should be unique
var user = users.Single(u => u.Id == userId);

// When ID might not exist
var user = users.SingleOrDefault(u => u.Id == userId);

// Wrong - allows duplicates
var user = users.First(u => u.UserName == userName);  // Might have duplicates!
```

## ElementAt and ElementAtOrDefault

### ElementAt - By Index

```csharp
try
{
    var third = employees.ElementAt(2);  // Zero-based index
    System.Console.WriteLine($"ElementAt(2): {third.FirstName} {third.LastName}");
}
catch (ArgumentOutOfRangeException)
{
    System.Console.WriteLine("ElementAt(2): sequence has fewer than 3 elements");
}
```

**What it does:**
- Returns element at specified zero-based index
- **Throws** `ArgumentOutOfRangeException` if index out of range
- Optimized for `IList<T>` (O(1))

**Signature:**
```csharp
TSource ElementAt<TSource>(this IEnumerable<TSource> source, int index)
TSource ElementAt<TSource>(this IEnumerable<TSource> source, Index index)
```

**Performance:**
- `IList<T>`: O(1) - direct indexing
- `IEnumerable<T>`: O(n) - must enumerate n elements

### ElementAtOrDefault - Safe Index Access

```csharp
var outOfRange = employees.ElementAtOrDefault(1000);
System.Console.WriteLine(outOfRange is not null
    ? $"ElementAtOrDefault(1000): {outOfRange.FirstName} {outOfRange.LastName}"
    : "ElementAtOrDefault(1000): <null>");
```

**What it does:**
- Returns element at index or `default(T)` if out of range
- **Never throws**

**When to use:**
```csharp
// When index might be out of range
var item = list.ElementAtOrDefault(index) ?? defaultItem;

// Prefer direct indexing when possible
if (index >= 0 && index < list.Count)
    var item = list[index];
```

### C# 8+ Index Syntax

```csharp
// From end with Index
var last = employees.ElementAt(^1);       // Last element
var secondLast = employees.ElementAt(^2); // Second to last

// Range
var slice = employees.Take(5..10);  // Elements 5-9
```

## DefaultIfEmpty

### Providing Fallback

```csharp
var emptySeq = employees.Where(e => e.EmployeeId == missingId).Select(e => e.FirstName);

var firstOrFallback = emptySeq.DefaultIfEmpty("<no employees>").First();
System.Console.WriteLine($"DefaultIfEmpty -> First(): {firstOrFallback}");
```

**What it does:**
- Returns original sequence if it contains elements
- Returns single-element sequence with `default(T)` if empty
- Enables safe First/Last on potentially empty sequences

**Signature:**
```csharp
IEnumerable<TSource> DefaultIfEmpty<TSource>(this IEnumerable<TSource> source)
IEnumerable<TSource> DefaultIfEmpty<TSource>(this IEnumerable<TSource> source, TSource defaultValue)
```

**How it works:**
```csharp
// Non-empty sequence
var seq = new[] { 1, 2, 3 };
seq.DefaultIfEmpty(0);  // [1, 2, 3] - unchanged

// Empty sequence
var empty = Enumerable.Empty<int>();
empty.DefaultIfEmpty(0);  // [0] - single element with default value
```

**Use cases:**
```csharp
// Safe First() on potentially empty sequence
var first = employees
    .Where(e => e.Role == WorkRole.Manager)
    .DefaultIfEmpty(new Employee { FirstName = "None" })
    .First();

// Left outer join pattern
var leftJoin = customers
    .GroupJoin(orders, c => c.Id, o => o.CustomerId, (c, orders) => new { c, orders })
    .SelectMany(x => x.orders.DefaultIfEmpty(), (x, order) => new
    {
        Customer = x.c.Name,
        OrderId = order?.Id ?? 0
    });
```

## Functional Programming Concepts

### 1. Short-Circuit Evaluation

Element operations stop as soon as result is found:

```csharp
var first = hugeSequence
    .Where(x => ExpensiveCheck(x))  // Stops after first match
    .First();

// Only evaluates ExpensiveCheck until first match
```

### 2. Option/Maybe Pattern

`FirstOrDefault`, `SingleOrDefault`, `ElementAtOrDefault` implement the **Option** pattern:

```csharp
// Returns T or null (C# equivalent of Option<T>)
var result = sequence.FirstOrDefault(predicate);

if (result is not null)
{
    // Some(value)
    Process(result);
}
else
{
    // None
    HandleMissing();
}
```

In functional languages like F# or Haskell:
```fsharp
// F#
let result = List.tryFind predicate list  // returns Option<T>
match result with
| Some value -> processValue value
| None -> handleMissing()
```

### 3. Explicit Error Handling

Distinction between "might not exist" vs "must exist":

```csharp
// Explicitly expect element (fail fast)
var config = configs.Single(c => c.IsDefault);

// Explicitly handle absence
var config = configs.SingleOrDefault(c => c.IsDefault) 
    ?? CreateDefaultConfig();
```

### 4. Lazy Evaluation

Element operations maintain deferred execution until called:

```csharp
var query = employees
    .Where(e => e.HireDate.Year > 2020)
    .Select(e => e.FirstName);  // Not executed

var first = query.First();  // NOW executes (only until first found)
```

## Element Operation Patterns

### Pattern 1: Safe First with Fallback

```csharp
var first = items.FirstOrDefault() ?? new Item();
```

### Pattern 2: Ensure Unique

```csharp
// Throws if duplicate IDs exist
var user = users.Single(u => u.Id == userId);
```

### Pattern 3: Pagination

```csharp
var page = items
    .Skip(pageIndex * pageSize)
    .Take(pageSize)
    .ToList();

// Safe access if page might be out of range
var firstOnPage = items.ElementAtOrDefault(pageIndex * pageSize);
```

### Pattern 4: Get Last N Items

```csharp
// Using Take with ^
var lastThree = items.TakeLast(3);

// Or with index
var lastThree = items.Skip(Math.Max(0, items.Count() - 3));
```

### Pattern 5: Default Value for Empty

```csharp
var avg = numbers
    .DefaultIfEmpty(0)
    .Average();  // Won't throw on empty
```

### Pattern 6: Assert Non-Empty

```csharp
// Throws if empty, documenting expectation
var first = items.First();  // "This should never be empty"
```

### Pattern 7: Handle Existence

```csharp
var item = items.SingleOrDefault(predicate);
if (item is null)
{
    // Not found - handle
    CreateNew();
}
else
{
    // Found - use
    Process(item);
}
```

## Performance Considerations

### IList vs IEnumerable

```csharp
// IList<T> (List, Array)
var list = new List<int> { 1, 2, 3, 4, 5 };
var last = list.Last();         // O(1) - uses list.Count and indexing
var at3 = list.ElementAt(3);    // O(1) - direct indexing

// IEnumerable<T>
var enumerable = GetNumbers();  // Returns IEnumerable
var last = enumerable.Last();   // O(n) - must enumerate all
var at3 = enumerable.ElementAt(3);  // O(n) - must enumerate 4 elements
```

### Materialize for Multiple Access

```csharp
// ❌ Bad - enumerates twice
var first = expensiveQuery.First();
var last = expensiveQuery.Last();

// ✅ Better - enumerate once
var list = expensiveQuery.ToList();
var first = list.First();
var last = list.Last();
```

### Early Termination

```csharp
// First/FirstOrDefault stop early
var first = hugeSequence.First(expensive Check);  // Stops after first match

// Count checks everything
var count = hugeSequence.Count(expensiveCheck);  // Checks all elements
```

## Common Mistakes

### 1. Using First When Single Expected

```csharp
// ❌ Wrong - silently succeeds with duplicates
var user = users.First(u => u.Email == email);

// ✅ Correct - throws if duplicates (data integrity issue)
var user = users.Single(u => u.Email == email);
```

### 2. Not Handling Default

```csharp
// ❌ Wrong - NullReferenceException if not found
var name = users.FirstOrDefault(u => u.Id == id).Name;

// ✅ Correct - null check
var user = users.FirstOrDefault(u => u.Id == id);
var name = user?.Name ?? "Unknown";
```

### 3. Expensive Last on IEnumerable

```csharp
// ❌ Inefficient - enumerates entire sequence
var last = expensiveQuery.Last();

// ✅ Better - order descending and take first
var last = expensiveQuery.OrderByDescending(x => x.Date).First();
```

### 4. Multiple Element Checks

```csharp
// ❌ Wrong - multiple enumerations
if (items.Any())
{
    var first = items.First();  // Enumerates again
}

// ✅ Better - use FirstOrDefault
var first = items.FirstOrDefault();
if (first is not null)
{
    // Use first
}
```

### 5. Forgetting Zero-Based Index

```csharp
// ❌ Wrong - gets second element
var first = items.ElementAt(1);

// ✅ Correct - gets first element
var first = items.ElementAt(0);
// Or better
var first = items.First();
```

## Comparison Table

| Method | Empty | Not Found | Multiple | Returns | Throws |
|--------|-------|-----------|----------|---------|--------|
| `First` | ✗ | ✗ | First | T | Yes |
| `FirstOrDefault` | default | default | First | T? | No |
| `Last` | ✗ | ✗ | Last | T | Yes |
| `LastOrDefault` | default | default | Last | T? | No |
| `Single` | ✗ | ✗ | ✗ | T | Yes |
| `SingleOrDefault` | default | default | ✗ | T? | Yes |
| `ElementAt` | ✗ | - | At index | T | Yes |
| `ElementAtOrDefault` | default | - | At index | T? | No |

✗ = Throws exception  
default = Returns `default(T)`

## Conclusion

LINQ element operations provide safe, expressive ways to access sequence elements:

- ✅ **First/Last**: Get first/last element (throw if empty)
- ✅ **FirstOrDefault/LastOrDefault**: Safe with default values
- ✅ **Single/SingleOrDefault**: Ensure exactly one match
- ✅ **ElementAt/ElementAtOrDefault**: Access by index
- ✅ **DefaultIfEmpty**: Provide fallback for empty sequences
- ✅ **Short-circuiting**: Stop as soon as element found
- ✅ **Explicit intent**: Clear difference between "must exist" and "might not exist"

The examples in [LinqElementOps.cs](LinqElementOps.cs) demonstrate:
- First and FirstOrDefault for initial element access
- LastOrDefault with ordering for deterministic results
- Single for ensuring unique matches
- ElementAt and ElementAtOrDefault for index-based access
- DefaultIfEmpty for providing fallback values

Element operations are essential for extracting specific values from sequences in a type-safe, declarative manner with explicit control over error handling.
