# LINQ Aggregation Operations

## Overview

Aggregation operations in LINQ process a sequence of values and reduce them to a single result. These operations implement the **fold** or **reduce** pattern from functional programming, transforming entire collections into summary values. Aggregations are essential for computing statistics, totals, and combining data.

## The Aggregate Operation

### Basic Aggregation

```csharp
var lastNames = employees.Select(e => e.LastName);
var concatenatedNames = lastNames.Aggregate((acc, ln) => acc + ", " + ln);
System.Console.WriteLine($"All last names: {concatenatedNames}");
```

**What it does:**
- Takes a sequence of values
- Applies a binary function repeatedly
- Reduces to a single result

**How it works:**
```
employees: [Smith, Jones, Davis, Wilson]
Step 1: "Smith" + ", " + "Jones"    → "Smith, Jones"
Step 2: "Smith, Jones" + ", " + "Davis" → "Smith, Jones, Davis"
Step 3: "Smith, Jones, Davis" + ", " + "Wilson" → "Smith, Jones, Davis, Wilson"
Result: "Smith, Jones, Davis, Wilson"
```

**Signature:**
```csharp
TSource Aggregate<TSource>(
    this IEnumerable<TSource> source,
    Func<TSource, TSource, TSource> func)
```

**Parameters:**
- `acc`: Accumulator (running result)
- `ln`: Current element
- Returns: New accumulator value

### Aggregation with Seed

```csharp
var firstNames = employees.Select(e => e.FirstName);
var concatenatedWithSeed = firstNames.Aggregate(
    "Employees: ",  // seed
    (acc, fn) => acc + fn + "; ");
System.Console.WriteLine(concatenatedWithSeed);
```

**What it does:**
- Starts with an initial **seed** value
- Processes each element
- Accumulator type can differ from element type

**How it works:**
```
seed: "Employees: "
employees: [John, Mary, Bob]
Step 1: "Employees: " + "John" + "; " → "Employees: John; "
Step 2: "Employees: John; " + "Mary" + "; " → "Employees: John; Mary; "
Step 3: "Employees: John; Mary; " + "Bob" + "; " → "Employees: John; Mary; Bob; "
Result: "Employees: John; Mary; Bob; "
```

**Signature:**
```csharp
TAccumulate Aggregate<TSource, TAccumulate>(
    this IEnumerable<TSource> source,
    TAccumulate seed,
    Func<TAccumulate, TSource, TAccumulate> func)
```

**Key insight:** The seed allows different accumulator and element types.

### Aggregate with Result Selector

```csharp
var result = employees.Aggregate(
    0,                           // seed
    (count, emp) => count + 1,   // accumulator
    total => $"Total: {total}"); // result selector
```

**Signature:**
```csharp
TResult Aggregate<TSource, TAccumulate, TResult>(
    this IEnumerable<TSource> source,
    TAccumulate seed,
    Func<TAccumulate, TSource, TAccumulate> func,
    Func<TAccumulate, TResult> resultSelector)
```

## Statistical Aggregations

### Average

```csharp
var averageHireYear = employees.Average(e => e.HireDate.Year);
System.Console.WriteLine($"Average hire year: {averageHireYear}");
```

**What it does:**
- Computes arithmetic mean
- Returns `double` (or nullable for empty sequences)

**Overloads:**
```csharp
double Average(this IEnumerable<int> source)
double Average<T>(this IEnumerable<T> source, Func<T, int> selector)
// Also: long, float, double, decimal versions
```

**Functional equivalent:**
```csharp
var avg = employees
    .Select(e => e.HireDate.Year)
    .Aggregate(
        (sum: 0.0, count: 0),
        (acc, year) => (acc.sum + year, acc.count + 1),
        acc => acc.sum / acc.count);
```

### Sum

```csharp
var totalYears = employees.Sum(e => e.HireDate.Year);
System.Console.WriteLine($"Sum of all hire years: {totalYears}");
```

**What it does:**
- Computes total sum
- Returns same type as input (int, long, double, etc.)

**Overloads:**
```csharp
int Sum(this IEnumerable<int> source)
int Sum<T>(this IEnumerable<T> source, Func<T, int> selector)
// Also: long, float, double, decimal versions
```

**Functional equivalent:**
```csharp
var sum = employees
    .Select(e => e.HireDate.Year)
    .Aggregate(0, (acc, year) => acc + year);
```

### Min and Max

```csharp
var minHireYear = employees.Min(e => e.HireDate.Year);
var maxHireYear = employees.Max(e => e.HireDate.Year);
System.Console.WriteLine($"First hire year: {minHireYear}");
System.Console.WriteLine($"Last hire year: {maxHireYear}");
```

**What they do:**
- `Min`: Returns smallest value
- `Max`: Returns largest value
- Works with any `IComparable<T>`

**Overloads:**
```csharp
TSource Min<TSource>(this IEnumerable<TSource> source)
TResult Min<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
```

**Functional equivalent:**
```csharp
var min = employees
    .Select(e => e.HireDate.Year)
    .Aggregate((minYear, year) => year < minYear ? year : minYear);
```

## Counting Operations

### Count

```csharp
var employeeCount = employees.Count();
System.Console.WriteLine($"Total employees: {employeeCount}");
```

**What it does:**
- Returns number of elements
- Returns `int`
- Optimized for `ICollection<T>` (uses `.Count` property)

**With predicate:**
```csharp
var vetsCount = employees.Count(e => e.Role == WorkRole.Veterinarian);
```

**Signature:**
```csharp
int Count<TSource>(this IEnumerable<TSource> source)
int Count<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
```

### LongCount

```csharp
var longCount = employees.LongCount();
System.Console.WriteLine($"Total employees (long): {longCount}");
```

**What it does:**
- Same as `Count` but returns `long`
- Use when count might exceed `int.MaxValue` (2,147,483,647)

**When to use:**
- Large datasets (billions of elements)
- When result type must be `long`

## Aggregating Nested Collections

```csharp
var totalCreditCards = employees.Sum(e => e.CreditCards.Count);
System.Console.WriteLine($"Total credit cards across all employees: {totalCreditCards}");
```

**What it does:**
- Aggregates a property from nested collections
- First projects each employee to their card count
- Then sums those counts

**Equivalent forms:**
```csharp
// Using Sum with selector
var total1 = employees.Sum(e => e.CreditCards.Count);

// Using SelectMany and Count
var total2 = employees
    .SelectMany(e => e.CreditCards)
    .Count();

// Using Aggregate
var total3 = employees
    .Aggregate(0, (acc, e) => acc + e.CreditCards.Count);
```

## Functional Programming Concepts

### 1. Fold/Reduce Pattern

`Aggregate` is the LINQ name for the **fold** (Haskell) or **reduce** (JavaScript, Python) operation:

```csharp
// Haskell: foldl (+) 0 [1,2,3,4] = 10
var sum = numbers.Aggregate(0, (acc, n) => acc + n);

// JavaScript: [1,2,3,4].reduce((acc, n) => acc + n, 0) = 10
var sum = numbers.Aggregate(0, (acc, n) => acc + n);
```

**Universal pattern:**
```
result = fold(function, seed, collection)
```

### 2. Left Fold

`Aggregate` is a **left fold** (processes left to right):

```csharp
// [1, 2, 3, 4]
// ((((seed) op 1) op 2) op 3) op 4)

var result = new[] { 1, 2, 3, 4 }
    .Aggregate("", (acc, n) => $"({acc} + {n})");
// Result: "(((( + 1) + 2) + 3) + 4)"
```

### 3. Accumulator Pattern

The accumulator carries state through the iteration:

```csharp
// Count elements
var count = items.Aggregate(0, (acc, item) => acc + 1);

// Build string
var str = items.Aggregate("", (acc, item) => acc + item.ToString());

// Find max
var max = items.Aggregate((acc, item) => item > acc ? item : acc);
```

### 4. Separation of Concerns

Aggregation separates:
- **What** to do (the function)
- **How** to iterate (handled by LINQ)

```csharp
// You specify WHAT
(acc, e) => acc + e.CreditCards.Count

// LINQ handles HOW
foreach (var e in employees)
{
    accumulator = func(accumulator, e);
}
```

### 5. Immutability

Each step creates a new value (conceptually):

```csharp
// Immutable accumulation
var result = numbers.Aggregate(
    ImmutableList<int>.Empty,
    (acc, n) => acc.Add(n * 2));
```

## Common Aggregation Patterns

### Pattern 1: Sum of Property

```csharp
var totalSalaries = employees.Sum(e => e.Salary);
```

### Pattern 2: Average with Filter

```csharp
var avgVetYears = employees
    .Where(e => e.Role == WorkRole.Veterinarian)
    .Average(e => (DateTime.Now - e.HireDate).TotalDays / 365);
```

### Pattern 3: Custom Aggregation

```csharp
// Most senior employee
var mostSenior = employees.Aggregate(
    (oldest, emp) => emp.HireDate < oldest.HireDate ? emp : oldest);
```

### Pattern 4: Building Complex Objects

```csharp
var stats = employees.Aggregate(
    new { Count = 0, TotalCards = 0, MinYear = int.MaxValue },
    (acc, e) => new
    {
        Count = acc.Count + 1,
        TotalCards = acc.TotalCards + e.CreditCards.Count,
        MinYear = Math.Min(acc.MinYear, e.HireDate.Year)
    });
```

### Pattern 5: String Concatenation

```csharp
// Without seed
var names1 = employees
    .Select(e => e.FirstName)
    .Aggregate((acc, name) => acc + ", " + name);

// With seed
var names2 = employees
    .Select(e => e.FirstName)
    .Aggregate("Names: ", (acc, name) => acc + name + "; ");

// Better: String.Join
var names3 = string.Join(", ", employees.Select(e => e.FirstName));
```

### Pattern 6: Composite Statistics

```csharp
var stats = employees
    .GroupBy(e => e.Role)
    .Select(g => new
    {
        Role = g.Key,
        Count = g.Count(),
        AvgYear = g.Average(e => e.HireDate.Year),
        TotalCards = g.Sum(e => e.CreditCards.Count),
        MinDate = g.Min(e => e.HireDate),
        MaxDate = g.Max(e => e.HireDate)
    });
```

## When to Use Each Operation

| Operation | Use When | Returns |
|-----------|----------|---------|
| `Aggregate` | Custom reduction logic | Any type |
| `Sum` | Adding numbers | Same as input type |
| `Average` | Computing mean | double |
| `Min` / `Max` | Finding extremes | Same as input type |
| `Count` | Counting items (< 2B) | int |
| `LongCount` | Counting items (> 2B) | long |

## Performance Considerations

### Count vs Count()

```csharp
// If source is ICollection<T>
List<int> list = new() { 1, 2, 3 };
var c1 = list.Count;        // O(1) - property access
var c2 = list.Count();      // O(1) - optimized to use Count property

// If source is IEnumerable<T>
IEnumerable<int> enumerable = GetNumbers();
var c3 = enumerable.Count();  // O(n) - must enumerate
```

### Materialization

Most aggregations must enumerate the entire sequence:

```csharp
// Must process all employees
var avg = employees.Average(e => e.HireDate.Year);

// Cannot stop early
// (unlike First or Any which can short-circuit)
```

### Multiple Aggregations

Avoid multiple passes:

```csharp
// ❌ Bad - enumerates 4 times
var count = employees.Count();
var sum = employees.Sum(e => e.HireDate.Year);
var min = employees.Min(e => e.HireDate.Year);
var max = employees.Max(e => e.HireDate.Year);

// ✅ Better - enumerate once
var stats = employees.Aggregate(
    new { Count = 0, Sum = 0, Min = int.MaxValue, Max = int.MinValue },
    (acc, e) => new
    {
        Count = acc.Count + 1,
        Sum = acc.Sum + e.HireDate.Year,
        Min = Math.Min(acc.Min, e.HireDate.Year),
        Max = Math.Max(acc.Max, e.HireDate.Year)
    });
```

## Common Mistakes

### 1. Empty Sequence Exceptions

```csharp
// ❌ Throws if empty
var avg = employees.Average(e => e.HireDate.Year);
var min = employees.Min(e => e.HireDate.Year);

// ✅ Safe with DefaultIfEmpty
var avg = employees
    .DefaultIfEmpty()
    .Average(e => e?.HireDate.Year ?? 0);

// ✅ Check first
if (employees.Any())
{
    var avg = employees.Average(e => e.HireDate.Year);
}
```

### 2. Wrong Aggregate Signature

```csharp
// ❌ Wrong - returns string for numbers
var sum = numbers.Aggregate("0", (acc, n) => acc + n.ToString());

// ✅ Correct - use int seed and accumulator
var sum = numbers.Aggregate(0, (acc, n) => acc + n);
```

### 3. Inefficient String Concatenation

```csharp
// ❌ Inefficient - creates many intermediate strings
var result = strings.Aggregate((acc, s) => acc + s);

// ✅ Better - use StringBuilder
var result = strings.Aggregate(
    new StringBuilder(),
    (sb, s) => sb.Append(s),
    sb => sb.ToString());

// ✅ Best - use string.Join
var result = string.Join("", strings);
```

### 4. Forgetting Null Checks

```csharp
// ❌ Can throw NullReferenceException
var totalCards = employees.Sum(e => e.CreditCards.Count);

// ✅ Safe
var totalCards = employees.Sum(e => e.CreditCards?.Count ?? 0);
```

## Advanced Patterns

### Parallel Aggregation

```csharp
// Aggregate with thread-safe operations
var total = employees
    .AsParallel()
    .Aggregate(
        0,                                    // seed per thread
        (acc, e) => acc + e.CreditCards.Count, // local accumulation
        (total1, total2) => total1 + total2,  // combine threads
        result => result);                    // final result
```

### Conditional Aggregation

```csharp
var conditionalSum = employees.Aggregate(
    0,
    (acc, e) => e.Role == WorkRole.Veterinarian
        ? acc + 1
        : acc);

// Equivalent to Count with predicate
var count = employees.Count(e => e.Role == WorkRole.Veterinarian);
```

## Conclusion

Aggregation operations reduce sequences to single values:

- ✅ **Aggregate**: Universal fold/reduce operation
- ✅ **Sum**: Add numeric values
- ✅ **Average**: Compute mean
- ✅ **Min/Max**: Find extremes
- ✅ **Count/LongCount**: Count elements
- ✅ **Functional**: Implements fold pattern
- ✅ **Composable**: Works with other LINQ operations

The examples in [LinqAggregation.cs](LinqAggregation.cs) demonstrate:
- Aggregate without seed (concatenating last names)
- Aggregate with seed (concatenating first names with prefix)
- Average, Sum, Min, Max on hire years
- Count and LongCount for employee totals
- Summing nested collection counts (credit cards)

Aggregations are essential for summarizing data, computing statistics, and reducing collections to meaningful single values in a functional, declarative style.
