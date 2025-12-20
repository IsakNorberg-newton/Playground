# LINQ Ordering Operations

## Overview

Ordering operations in LINQ sort sequences based on one or more keys. Sorting is essential for presenting data in a meaningful order and is a staple of data processing. LINQ provides a fluent, composable way to order data that integrates seamlessly with other operations.

## Primary Ordering Operations

### OrderBy - Ascending Sort

```csharp
var byName = employees.OrderBy(e => e.LastName).ThenBy(e => e.FirstName);
System.Console.WriteLine("Ordered by LastName, ThenBy FirstName (showing 10):");
byName.Take(10).ToList().ForEach(e => System.Console.WriteLine($"{e.LastName}, {e.FirstName} - {e.Role}"));
```

**What it does:**
- Sorts employees by `LastName` in ascending order
- Then sorts by `FirstName` for employees with same last name
- Returns `IOrderedEnumerable<Employee>`

**Key Points:**
- Takes a key selector function (`Func<T, TKey>`)
- Uses default comparer for the key type
- Preserves stable sort (equal elements maintain relative order)

### OrderByDescending - Descending Sort

```csharp
var byHireDesc = employees.OrderByDescending(e => e.HireDate).ThenByDescending(e => e.LastName);
System.Console.WriteLine("Ordered by HireDate desc, ThenByDescending LastName (showing 10):");
byHireDesc.Take(10).ToList().ForEach(e => System.Console.WriteLine($"{e.HireDate:d} - {e.FirstName} {e.LastName}"));
```

**What it does:**
- Sorts by `HireDate` in descending order (most recent first)
- Then sorts by `LastName` descending for same hire dates
- Useful for "newest first" or "highest to lowest" scenarios

## Secondary Ordering Operations

### ThenBy - Secondary Ascending Sort

```csharp
employees.OrderBy(e => e.LastName).ThenBy(e => e.FirstName);
```

**What it does:**
- Provides secondary sort criterion
- Only affects elements with equal primary keys
- Can chain multiple `ThenBy` calls

**Important**: Must be called on `IOrderedEnumerable<T>` (result of `OrderBy`/`OrderByDescending`)

### ThenByDescending - Secondary Descending Sort

```csharp
employees.OrderByDescending(e => e.HireDate).ThenByDescending(e => e.LastName);
```

**What it does:**
- Secondary sort in descending order
- Combines with primary sort direction

## The Reverse Operation

```csharp
var reversed = byName.Take(10).Reverse();
System.Console.WriteLine("Reversed first 10 from previous ordering:");
reversed.ToList().ForEach(e => System.Console.WriteLine($"{e.LastName}, {e.FirstName}"));
```

**What it does:**
- Reverses the order of elements in the sequence
- Different from `OrderByDescending`—it reverses the existing order
- Works on any `IEnumerable<T>`, not just ordered sequences

**Use Cases:**
- Reverse chronological order
- Display in opposite order
- LIFO (Last In, First Out) access

## IOrderedEnumerable&lt;T&gt;

`OrderBy` and `OrderByDescending` return a special interface:

```csharp
IOrderedEnumerable<Employee> ordered = employees.OrderBy(e => e.LastName);
```

**Why Special?**
- Enables `ThenBy` and `ThenByDescending`
- Maintains ordering information for subsequent sorts
- Still implements `IEnumerable<T>` (can be used anywhere)

**Incorrect Usage:**
```csharp
// ❌ Wrong - ThenBy requires IOrderedEnumerable
var wrong = employees
    .Where(e => e.Role == WorkRole.Veterinarian)  // Returns IEnumerable<T>
    .ThenBy(e => e.FirstName);  // Compile error!

// ✅ Correct - OrderBy first
var correct = employees
    .Where(e => e.Role == WorkRole.Veterinarian)
    .OrderBy(e => e.LastName)     // Returns IOrderedEnumerable<T>
    .ThenBy(e => e.FirstName);    // Now works!
```

## Ordering Examples

### Single Key

```csharp
// By hire date
var byHireDate = employees.OrderBy(e => e.HireDate);

// By role
var byRole = employees.OrderBy(e => e.Role);
```

### Multiple Keys

```csharp
// Primary: LastName, Secondary: FirstName
var byFullName = employees
    .OrderBy(e => e.LastName)
    .ThenBy(e => e.FirstName);

// Primary: Role desc, Secondary: HireDate, Tertiary: LastName
var complex = employees
    .OrderByDescending(e => e.Role)
    .ThenBy(e => e.HireDate)
    .ThenBy(e => e.LastName);
```

### Computed Keys

```csharp
// Order by years employed (calculated)
var bySeniority = employees.OrderByDescending(e => 
    (DateTime.Now - e.HireDate).TotalDays
);

// Order by number of credit cards
var byCardCount = employees.OrderBy(e => e.CreditCards.Count);
```

## Functional Programming Concepts

### 1. Composability

Ordering integrates seamlessly with other operations:

```csharp
var result = employees
    .Where(e => e.Role == WorkRole.Veterinarian)  // Filter
    .OrderBy(e => e.LastName)                     // Order
    .ThenBy(e => e.FirstName)                     // Secondary order
    .Select(e => new { e.FirstName, e.LastName }) // Project
    .Take(10);                                     // Limit
```

Each operation returns an enumerable, enabling unlimited chaining.

### 2. Immutability

Ordering creates a new ordered view without modifying the original:

```csharp
var original = employees;
var ordered = employees.OrderBy(e => e.LastName);

// original remains in its original order
// ordered provides a sorted view
```

### 3. Lazy Evaluation

Ordering is deferred until enumeration:

```csharp
var query = employees.OrderBy(e => e.LastName);  // Not sorted yet

foreach (var e in query)  // NOW it sorts
{
    Console.WriteLine(e.LastName);
}
```

**Benefit**: Build complex queries step by step, sort executes with current data.

### 4. Stable Sort

LINQ ordering is **stable**—elements with equal keys maintain their original relative order:

```csharp
var items = new[] { (Name: "Alice", Age: 30), (Name: "Bob", Age: 30), (Name: "Charlie", Age: 30) };
var ordered = items.OrderBy(x => x.Age);

// Order preserved for equal ages: Alice, Bob, Charlie
```

## Ordering Patterns

### Pattern 1: Reverse Chronological

```csharp
// Most recent first
var newest = employees.OrderByDescending(e => e.HireDate);
```

### Pattern 2: Alphabetical with Case-Insensitivity

```csharp
var alphabetical = employees.OrderBy(e => e.LastName, StringComparer.OrdinalIgnoreCase);
```

### Pattern 3: Natural Ordering (Strings with Numbers)

```csharp
// Custom comparer for natural sort: "Item1", "Item2", "Item10"
var natural = items.OrderBy(x => x, new NaturalStringComparer());
```

### Pattern 4: Null Handling

```csharp
// Nulls first
var nullsFirst = items.OrderBy(x => x.Value == null ? 0 : 1).ThenBy(x => x.Value);

// Nulls last
var nullsLast = items.OrderBy(x => x.Value == null ? 1 : 0).ThenBy(x => x.Value);
```

### Pattern 5: Complex Business Logic

```csharp
// Sort by priority: Admins first, then by seniority
var prioritized = employees
    .OrderByDescending(e => e.Role == WorkRole.Admin)
    .ThenByDescending(e => (DateTime.Now - e.HireDate).Days);
```

## Performance Considerations

### Time Complexity
- **OrderBy/OrderByDescending**: O(n log n)
- **ThenBy/ThenByDescending**: O(n log n) total (not additional)
- **Reverse**: O(n) - linear

### Memory
- Ordering must materialize the sequence (buffer elements)
- Cannot stream through unbounded sequences
- Consider memory usage for large datasets

### Best Practices

```csharp
// ✅ Good - order once, then filter/project
var result = employees
    .OrderBy(e => e.LastName)
    .Where(e => e.Role == WorkRole.Veterinarian)
    .Select(e => e.FirstName);

// ❌ Less efficient - ordering after projecting loses information
var result = employees
    .Select(e => e.FirstName)
    .OrderBy(name => name);  // Can only order by name now
```

## OrderBy vs Sort

| LINQ OrderBy | List.Sort() |
|--------------|-------------|
| Returns new sequence | Modifies in place |
| Immutable operation | Mutable operation |
| Lazy evaluation | Immediate execution |
| Composable | Terminal operation |
| Works on any `IEnumerable<T>` | Only on `List<T>` |

**When to use each:**
- Use `OrderBy` for LINQ queries and functional pipelines
- Use `Sort()` when you own the list and want in-place sorting

## Common Mistakes

### 1. Calling ThenBy Without OrderBy

```csharp
// ❌ Error - ThenBy requires IOrderedEnumerable
var wrong = employees.ThenBy(e => e.FirstName);

// ✅ Correct - OrderBy first
var correct = employees.OrderBy(e => e.LastName).ThenBy(e => e.FirstName);
```

### 2. Multiple Independent Sorts

```csharp
// ❌ Wrong - only last OrderBy applies
var wrong = employees
    .OrderBy(e => e.LastName)
    .OrderBy(e => e.FirstName);  // Overwrites previous sort!

// ✅ Correct - use ThenBy for secondary sort
var correct = employees
    .OrderBy(e => e.LastName)
    .ThenBy(e => e.FirstName);
```

### 3. Ordering Before Distinct

```csharp
// ❌ Inefficient - sorts all, then removes duplicates
var wrong = employees
    .OrderBy(e => e.LastName)
    .Select(e => e.LastName)
    .Distinct();

// ✅ Better - remove duplicates first, then sort fewer items
var correct = employees
    .Select(e => e.LastName)
    .Distinct()
    .OrderBy(name => name);
```

## Query Syntax

LINQ query syntax supports ordering with `orderby`:

```csharp
var query = from e in employees
            orderby e.LastName, e.FirstName
            select e;

// With descending
var query = from e in employees
            orderby e.HireDate descending, e.LastName descending
            select e;
```

**Equivalent method syntax:**
```csharp
var query = employees
    .OrderBy(e => e.LastName)
    .ThenBy(e => e.FirstName);

var query = employees
    .OrderByDescending(e => e.HireDate)
    .ThenByDescending(e => e.LastName);
```

## Conclusion

Ordering operations in LINQ provide a powerful, composable way to sort data:

- ✅ **OrderBy/OrderByDescending**: Primary sort
- ✅ **ThenBy/ThenByDescending**: Secondary and tertiary sorts
- ✅ **Reverse**: Reverse sequence order
- ✅ **Stable**: Equal elements maintain relative order
- ✅ **Composable**: Chain with other LINQ operations

The examples in [LinqOrdering.cs](LinqOrdering.cs) show:
- Single and multi-level sorting
- Ascending and descending orders
- Reversing ordered sequences

Ordering is essential for presenting data in a meaningful way and integrates seamlessly into functional pipelines, enabling readable, maintainable data processing code.
